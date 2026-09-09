using SubastaYa.Application.DTOs.Pujas;
using SubastaYa.Application.Interfaces;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;
using System.Transactions; // <-- Transaccionalidad ACID

namespace SubastaYa.Application.Services;

public class PujaService : IPujaService
{
    private readonly ISubastaRepository _subastaRepository;
    private readonly IPujaRepository _pujaRepository;
    private readonly IBilleteraRepository _billeteraRepository;
    private readonly ITransaccionLedgerRepository _ledgerRepository;
    private readonly IAuditoriaLogRepository _auditoriaRepository;

    public PujaService(
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

    public async Task<ResultadoPujaDto> RealizarPujaAsync(CrearPujaDto dto)
    {
        var ahora = DateTime.UtcNow;

        // =====================================================================
        // 1. VALIDACIONES CRÍTICAS DE NEGOCIO (CON AUDITORÍA DE RECHAZO)
        // =====================================================================

        // A. Validar existencia
        var subasta = await _subastaRepository.ObtenerPorIdAsync(dto.SubastaId);
        if (subasta == null)
        {
            await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, "Subasta no encontrada");
            throw new SubastaNoEncontradaException(dto.SubastaId);
        }

        // B. Validar estado
        if (subasta.Estado != EstadoSubasta.Activa)
        {
            await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, $"Subasta no activa (Estado: {subasta.Estado})");
            throw new SubastaNoActivaException(subasta.Estado.ToString());
        }

        // C. Validar horarios
        if (ahora < subasta.FechaInicio || ahora > subasta.FechaFin)
        {
            await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, "Subasta fuera de curso horario");
            throw new PujaInvalidaException("La subasta no se encuentra en curso.");
        }

        // D. El vendedor no puede auto-pujarse
        if (subasta.VendedorId == dto.CompradorId)
        {
            await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, "Vendedor intentó pujar en su propia subasta");
            throw new PujaInvalidaException("El vendedor no puede realizar ofertas en su propia subasta.");
        }

        // E. Validar montos contra puja líder actual
        var pujaLiderActual = await _pujaRepository.ObtenerPujaMasAltaAsync(dto.SubastaId);

        if (pujaLiderActual != null)
        {
            if (pujaLiderActual.CompradorId == dto.CompradorId)
            {
                await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, "El postor ya es el líder actual");
                throw new PujaInvalidaException("Ya eres el postor líder de esta subasta.");
            }

            var montoMinimoRequerido = pujaLiderActual.Monto + subasta.IncrementoMinimo;
            if (dto.Monto < montoMinimoRequerido)
            {
                await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, $"Monto menor al mínimo requerido (${montoMinimoRequerido:N2})");
                throw new PujaInvalidaException($"La oferta debe ser de al menos ${montoMinimoRequerido:N2} (Puja líder: ${pujaLiderActual.Monto:N2} + Incremento: ${subasta.IncrementoMinimo:N2}).");
            }
        }
        else
        {
            if (dto.Monto < subasta.PrecioBase)
            {
                await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, $"Monto menor al precio base (${subasta.PrecioBase:N2})");
                throw new PujaInvalidaException($"La primera oferta debe ser igual o superior al precio base de ${subasta.PrecioBase:N2}.");
            }
        }

        // F. Validar solvencia económica en la billetera
        var billeteraNuevoPostor = await _billeteraRepository.ObtenerPorUsuarioIdAsync(dto.CompradorId);
        if (billeteraNuevoPostor == null)
        {
            await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, "Billetera no encontrada");
            throw new DomainException($"No se encontró la billetera del usuario {dto.CompradorId}.");
        }

        if (billeteraNuevoPostor.SaldoDisponible < dto.Monto)
        {
            await AuditarRechazoAsync(dto.SubastaId, dto.CompradorId, dto.Monto, $"Saldo disponible insuficiente (${billeteraNuevoPostor.SaldoDisponible:N2})");
            throw new SaldoInsuficienteException(billeteraNuevoPostor.SaldoDisponible, dto.Monto);
        }

        // =====================================================================
        // 2. BLOQUE TRANSACCIONAL ATÓMICO (ACID - ESCROW & CONCURRENCIA)
        // =====================================================================
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        // A. Liberar garantía al líder anterior (si existía)
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

        // B. Bloquear saldo al nuevo postor (Garantía / Escrow)
        billeteraNuevoPostor.SaldoDisponible -= dto.Monto;
        billeteraNuevoPostor.SaldoRetenido += dto.Monto;
        billeteraNuevoPostor.Version++;

        await _billeteraRepository.ActualizarAsync(billeteraNuevoPostor);

        await _ledgerRepository.AgregarAsync(new TransaccionLedger
        {
            BilleteraId = billeteraNuevoPostor.Id,
            Tipo = TipoTransaccionLedger.Retencion,
            Monto = dto.Monto,
            Fecha = ahora,
            SubastaId = subasta.Id
        });

        // C. Registrar la nueva Puja
        var nuevaPuja = new Puja
        {
            SubastaId = subasta.Id,
            CompradorId = dto.CompradorId,
            Monto = dto.Monto,
            FechaPuja = ahora
        };
        await _pujaRepository.AgregarAsync(nuevaPuja);

        // D. Actualizar versión de Subasta SIEMPRE (Garantiza Optimistic Locking / 409 Conflict)
        subasta.Version++;

        // E. Regla Anti-Sniping (Extensión de 2 minutos si faltan <= 60s)
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
                UsuarioId = dto.CompradorId,
                DetalleJson = $"{{\"segundosRestantes\": {Math.Round(segundosRestantes, 2)}, \"nuevaFechaFin\": \"{subasta.FechaFin:O}\"}}",
                Fecha = ahora
            });
        }

        // Persistir la subasta con su nueva versión en SQL Server
        await _subastaRepository.ActualizarAsync(subasta);

        // Confirmar transacción atómica completa
        scope.Complete();

        return new ResultadoPujaDto
        {
            Exitosa = true,
            Mensaje = tiempoExtendido
                ? "¡Oferta líder registrada! Se extendió el tiempo por 2 minutos (Regla Anti-Sniping)."
                : "¡Oferta líder registrada exitosamente!",
            MontoOfertado = dto.Monto,
            TiempoExtendido = tiempoExtendido,
            NuevaFechaFin = subasta.FechaFin
        };
    }

    // Auditoría inmutable de rechazos requerida por la sección 3.4 del TP
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
            // El log de auditoría no debe tapar la excepción de negocio principal
        }
    }

    public async Task<IEnumerable<PujaDto>> ObtenerHistorialPorSubastaIdAsync(int subastaId)
    {
        var pujas = await _pujaRepository.ObtenerPorSubastaIdAsync(subastaId);

        return pujas.OrderByDescending(p => p.FechaPuja).Select(p => new PujaDto
        {
            Id = p.Id,
            SubastaId = p.SubastaId,
            CompradorId = p.CompradorId,
            CompradorSeudonimo = AnonimizarNombre(p.Comprador?.Nombre ?? "Usuario"),
            Monto = p.Monto,
            FechaPuja = p.FechaPuja
        });
    }

    private static string AnonimizarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length <= 2)
            return "Postor***";

        return $"{nombre[..2]}***{nombre[^1]}";
    }
}