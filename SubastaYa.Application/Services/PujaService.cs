using SubastaYa.Application.DTOs.Pujas;
using SubastaYa.Application.Interfaces;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;

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

        // 1. Validar existencia y estado de la Subasta
        var subasta = await _subastaRepository.ObtenerPorIdAsync(dto.SubastaId);
        if (subasta == null)
            throw new SubastaNoEncontradaException(dto.SubastaId);

        if (subasta.Estado != EstadoSubasta.Activa)
            throw new SubastaNoActivaException(subasta.Estado.ToString());

        if (ahora < subasta.FechaInicio || ahora > subasta.FechaFin)
            throw new PujaInvalidaException("La subasta no se encuentra en curso.");

        // 2. El vendedor no puede auto-pujarse
        if (subasta.VendedorId == dto.CompradorId)
            throw new PujaInvalidaException("El vendedor no puede realizar ofertas en su propia subasta.");

        // 3. Validar montos contra la última oferta
        var pujaLiderActual = await _pujaRepository.ObtenerPujaMasAltaAsync(dto.SubastaId);

        if (pujaLiderActual != null)
        {
            if (pujaLiderActual.CompradorId == dto.CompradorId)
                throw new PujaInvalidaException("Ya eres el postor líder de esta subasta.");

            var montoMinimoRequerido = pujaLiderActual.Monto + subasta.IncrementoMinimo;
            if (dto.Monto < montoMinimoRequerido)
                throw new PujaInvalidaException($"La oferta debe ser de al menos ${montoMinimoRequerido:N2} (Puja líder: ${pujaLiderActual.Monto:N2} + Incremento: ${subasta.IncrementoMinimo:N2}).");
        }
        else
        {
            if (dto.Monto < subasta.PrecioBase)
                throw new PujaInvalidaException($"La primera oferta debe ser igual o superior al precio base de ${subasta.PrecioBase:N2}.");
        }

        // 4. Validar solvencia del nuevo postor (Billetera)
        var billeteraNuevoPostor = await _billeteraRepository.ObtenerPorUsuarioIdAsync(dto.CompradorId);
        if (billeteraNuevoPostor == null)
            throw new DomainException($"No se encontró la billetera del usuario {dto.CompradorId}.");

        if (billeteraNuevoPostor.SaldoDisponible < dto.Monto)
            throw new SaldoInsuficienteException(billeteraNuevoPostor.SaldoDisponible, dto.Monto);

        // =====================================================================
        // 5. MANEJO ATÓMICO DE SALDOS (ESCROW / GARANTÍA)
        // =====================================================================

        // A. Si había un postor líder previo, LIBERAR su retención
        if (pujaLiderActual != null)
        {
            var billeteraPostorAnterior = await _billeteraRepository.ObtenerPorUsuarioIdAsync(pujaLiderActual.CompradorId);
            if (billeteraPostorAnterior != null)
            {
                billeteraPostorAnterior.SaldoRetenido -= pujaLiderActual.Monto;
                billeteraPostorAnterior.SaldoDisponible += pujaLiderActual.Monto;
                billeteraPostorAnterior.Version++;

                await _billeteraRepository.ActualizarAsync(billeteraPostorAnterior);

                // Asentar liberación en el Ledger
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

        // B. BLOQUEAR / RETENER saldo al nuevo postor
        billeteraNuevoPostor.SaldoDisponible -= dto.Monto;
        billeteraNuevoPostor.SaldoRetenido += dto.Monto;
        billeteraNuevoPostor.Version++;

        await _billeteraRepository.ActualizarAsync(billeteraNuevoPostor);

        // Asentar retención en el Ledger
        await _ledgerRepository.AgregarAsync(new TransaccionLedger
        {
            BilleteraId = billeteraNuevoPostor.Id,
            Tipo = TipoTransaccionLedger.Retencion,
            Monto = dto.Monto,
            Fecha = ahora,
            SubastaId = subasta.Id
        });

        // C. Registrar la nueva Puja ganadora
        var nuevaPuja = new Puja
        {
            SubastaId = subasta.Id,
            CompradorId = dto.CompradorId,
            Monto = dto.Monto,
            FechaPuja = ahora
        };
        await _pujaRepository.AgregarAsync(nuevaPuja);

        // =====================================================================
        // 6. REGLA ANTI-SNIPING (Extensión dinámica de tiempo)
        // =====================================================================
        bool tiempoExtendido = false;
        var segundosRestantes = (subasta.FechaFin - ahora).TotalSeconds;

        // Si la oferta entra dentro de los últimos 60 segundos:
        if (segundosRestantes > 0 && segundosRestantes <= 60)
        {
            subasta.FechaFin = subasta.FechaFin.AddMinutes(2);
            subasta.Version++;
            tiempoExtendido = true;

            await _subastaRepository.ActualizarAsync(subasta);

            // Auditoría obligatoria de la extensión
            await _auditoriaRepository.AgregarAsync(new AuditoriaLog
            {
                Entidad = "SUBASTA",
                EntidadId = subasta.Id,
                Accion = TipoAccionAuditoria.ExtensionTiempo.ToString(),
                UsuarioId = dto.CompradorId,
                DetalleJson = $"{{\"segundosRestantesAlOfertar\": {Math.Round(segundosRestantes, 2)}, \"nuevaFechaFin\": \"{subasta.FechaFin:O}\"}}",
                Fecha = ahora
            });
        }

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

    public async Task<IEnumerable<PujaDto>> ObtenerHistorialPorSubastaIdAsync(int subastaId)
    {
        var pujas = await _pujaRepository.ObtenerPorSubastaIdAsync(subastaId);

        return pujas.OrderByDescending(p => p.FechaPuja).Select(p => new PujaDto
        {
            Id = p.Id,
            SubastaId = p.SubastaId,
            CompradorId = p.CompradorId,
            // Anonimización del postor para la sala en vivo (ej: Com***ador 1)
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