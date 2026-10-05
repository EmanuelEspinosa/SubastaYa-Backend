using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Pujas;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Commands.Pujas
{
    public class RealizarPujaCommandHandler : ICommandHandler<RealizarPujaCommand, ResultadoPujaDto>
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IPujaRepository _pujaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;
        private readonly IAuditoriaLogRepository _auditoriaRepository;

        public RealizarPujaCommandHandler(
            ISubastaRepository subastaRepository,
            IPujaRepository pujaRepository,
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository,
            IAuditoriaLogRepository auditoriaRepository)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<ResultadoPujaDto> HandleAsync(RealizarPujaCommand command, CancellationToken cancellationToken = default)
        {
            var ahora = DateTime.UtcNow;

            // 1. VALIDACIONES CRÍTICAS DE NEGOCIO
            var subasta = await _subastaRepository.ObtenerPorIdAsync(command.SubastaId);
            if (subasta == null)
            {
                await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, "Subasta no encontrada");
                throw new SubastaNoEncontradaException(command.SubastaId);
            }

            if (subasta.Estado != EstadoSubasta.Activa)
            {
                await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, $"Subasta no activa (Estado: {subasta.Estado})");
                throw new SubastaNoActivaException(subasta.Estado.ToString());
            }

            if (ahora < subasta.FechaInicio || ahora > subasta.FechaFin)
            {
                await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, "Subasta fuera de curso horario");
                throw new PujaInvalidaException("La subasta no se encuentra en curso.");
            }

            if (subasta.VendedorId == command.CompradorId)
            {
                await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, "Vendedor intentó pujar en su propia subasta");
                throw new PujaInvalidaException("El vendedor no puede realizar ofertas en su propia subasta.");
            }

            var pujaLiderActual = await _pujaRepository.ObtenerPujaMasAltaAsync(command.SubastaId);

            if (pujaLiderActual != null)
            {
                if (pujaLiderActual.CompradorId == command.CompradorId)
                {
                    await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, "El postor ya es el líder actual");
                    throw new PujaInvalidaException("Ya eres el postor líder de esta subasta.");
                }

                var montoMinimoRequerido = pujaLiderActual.Monto + subasta.IncrementoMinimo;
                if (command.Monto < montoMinimoRequerido)
                {
                    await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, $"Monto menor al mínimo requerido (${montoMinimoRequerido:N2})");
                    throw new PujaInvalidaException($"La oferta debe ser de al menos ${montoMinimoRequerido:N2} (Puja líder: ${pujaLiderActual.Monto:N2} + Incremento: ${subasta.IncrementoMinimo:N2}).");
                }
            }
            else
            {
                if (command.Monto < subasta.PrecioBase)
                {
                    await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, $"Monto menor al precio base (${subasta.PrecioBase:N2})");
                    throw new PujaInvalidaException($"La primera oferta debe ser igual o superior al precio base de ${subasta.PrecioBase:N2}.");
                }
            }

            var billeteraNuevoPostor = await _billeteraRepository.ObtenerPorUsuarioIdAsync(command.CompradorId);
            if (billeteraNuevoPostor == null)
            {
                await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, "Billetera no encontrada");
                throw new DomainException($"No se encontró la billetera del usuario {command.CompradorId}.");
            }

            if (billeteraNuevoPostor.SaldoDisponible < command.Monto)
            {
                await AuditarRechazoAsync(command.SubastaId, command.CompradorId, command.Monto, $"Saldo disponible insuficiente (${billeteraNuevoPostor.SaldoDisponible:N2})");
                throw new SaldoInsuficienteException(billeteraNuevoPostor.SaldoDisponible, command.Monto);
            }

            // 2. BLOQUE TRANSACCIONAL ATÓMICO (ACID - ESCROW & CONCURRENCIA)
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // A. Liberar garantía al líder anterior
            if (pujaLiderActual != null)
            {
                var billeteraPostorAnterior = await _billeteraRepository.ObtenerPorUsuarioIdAsync(pujaLiderActual.CompradorId);
                if (billeteraPostorAnterior != null)
                {
                    billeteraPostorAnterior.SaldoRetenido -= pujaLiderActual.Monto;
                    billeteraPostorAnterior.SaldoDisponible += pujaLiderActual.Monto;
                    billeteraPostorAnterior.Version++;

                    await _billeteraRepository.ActualizarAsync(billeteraPostorAnterior);

                    await _ledgerRepository.AgregarAsync(new TransaccionLedger
                    {
                        BilleteraId = billeteraPostorAnterior.Id,
                        Tipo = TipoTransaccionLedger.Liberacion,
                        Monto = pujaLiderActual.Monto,
                        Fecha = ahora,
                        SubastaId = subasta.Id
                    });
                }
            }

            // B. Bloquear saldo al nuevo postor
            billeteraNuevoPostor.SaldoDisponible -= command.Monto;
            billeteraNuevoPostor.SaldoRetenido += command.Monto;
            billeteraNuevoPostor.Version++;

            await _billeteraRepository.ActualizarAsync(billeteraNuevoPostor);

            await _ledgerRepository.AgregarAsync(new TransaccionLedger
            {
                BilleteraId = billeteraNuevoPostor.Id,
                Tipo = TipoTransaccionLedger.Retencion,
                Monto = command.Monto,
                Fecha = ahora,
                SubastaId = subasta.Id
            });

            // C. Registrar nueva Puja
            var nuevaPuja = new Puja
            {
                SubastaId = subasta.Id,
                CompradorId = command.CompradorId,
                Monto = command.Monto,
                FechaPuja = ahora
            };
            await _pujaRepository.AgregarAsync(nuevaPuja);

            // D. Control de Concurrencia Optimista
            subasta.Version++;

            // E. Regla Anti-Sniping (Extensión de 2 minutos)
            bool tiempoExtendido = false;
            var segundosRestantes = (subasta.FechaFin - ahora).TotalSeconds;

            if (segundosRestantes > 0 && segundosRestantes <= 60)
            {
                subasta.FechaFin = subasta.FechaFin.AddMinutes(2);
                tiempoExtendido = true;

                await _auditoriaRepository.AgregarAsync(new AuditoriaLog
                {
                    Entidad = "SUBASTA",
                    EntidadId = subasta.Id,
                    Accion = "EXTENSION_TIEMPO",
                    UsuarioId = command.CompradorId,
                    DetalleJson = $"{{\"segundosRestantes\": {Math.Round(segundosRestantes, 2)}, \"nuevaFechaFin\": \"{subasta.FechaFin:O}\"}}",
                    Fecha = ahora
                });
            }

            await _subastaRepository.ActualizarAsync(subasta);
            scope.Complete();

            return new ResultadoPujaDto
            {
                Exitosa = true,
                Mensaje = tiempoExtendido
                    ? "¡Oferta líder registrada! Se extendió el tiempo por 2 minutos (Regla Anti-Sniping)."
                    : "¡Oferta líder registrada exitosamente!",
                MontoOfertado = command.Monto,
                TiempoExtendido = tiempoExtendido,
                NuevaFechaFin = subasta.FechaFin
            };
        }

        private async Task AuditarRechazoAsync(int subastaId, int compradorId, decimal monto, string motivo)
        {
            try
            {
                await _auditoriaRepository.AgregarAsync(new AuditoriaLog
                {
                    Entidad = "SUBASTA",
                    EntidadId = subastaId,
                    Accion = "PUJA_RECHAZADA",
                    UsuarioId = compradorId,
                    DetalleJson = $"{{\"montoOfertado\": {monto}, \"motivo\": \"{motivo}\"}}",
                    Fecha = DateTime.UtcNow
                });
            }
            catch
            {
                // Ignorar excepción secundaría de auditoría
            }
        }
    }
}
