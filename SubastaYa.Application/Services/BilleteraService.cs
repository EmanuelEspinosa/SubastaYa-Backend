using SubastaYa.Application.DTOs.Billetera;
using SubastaYa.Application.Interfaces;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Services;

public class BilleteraService : IBilleteraService
{
    private readonly IBilleteraRepository _billeteraRepository;
    private readonly ITransaccionLedgerRepository _ledgerRepository;
    private readonly IAuditoriaLogRepository _auditoriaRepository;

    public BilleteraService(
        IBilleteraRepository billeteraRepository,
        ITransaccionLedgerRepository ledgerRepository,
        IAuditoriaLogRepository auditoriaRepository)
    {
        _billeteraRepository = billeteraRepository;
        _ledgerRepository = ledgerRepository;
        _auditoriaRepository = auditoriaRepository;
    }

    public async Task<BilleteraDto> ObtenerSaldoPorUsuarioIdAsync(int usuarioId)
    {
        var billetera = await _billeteraRepository.ObtenerPorUsuarioIdAsync(usuarioId);
        if (billetera == null)
            throw new DomainException($"No se encontró la billetera del usuario {usuarioId}.");

        return new BilleteraDto
        {
            Id = billetera.Id,
            UsuarioId = billetera.UsuarioId,
            SaldoTotal = billetera.SaldoTotal,
            SaldoRetenido = billetera.SaldoRetenido,
            SaldoDisponible = billetera.SaldoDisponible
        };
    }

    public async Task<BilleteraDto> CargarSaldoAsync(CargarSaldoDto dto)
    {
        if (dto.Monto <= 0)
            throw new DomainException("El monto a cargar debe ser mayor a cero.");

        var billetera = await _billeteraRepository.ObtenerPorUsuarioIdAsync(dto.UsuarioId);
        if (billetera == null)
            throw new DomainException($"No se encontró la billetera del usuario {dto.UsuarioId}.");

        // Actualizar saldos
        billetera.SaldoTotal += dto.Monto;
        billetera.SaldoDisponible += dto.Monto;
        billetera.Version++;

        await _billeteraRepository.ActualizarAsync(billetera);

        // Registrar en el Libro Mayor (Ledger)
        await _ledgerRepository.AgregarAsync(new TransaccionLedger
        {
            BilleteraId = billetera.Id,
            Tipo = TipoTransaccionLedger.Deposito,
            Monto = dto.Monto,
            Fecha = DateTime.UtcNow
        });

        // Registrar Auditoría obligatoria
        await _auditoriaRepository.AgregarAsync(new AuditoriaLog
        {
            Entidad = "BILLETERA",
            EntidadId = billetera.Id,
            Accion = TipoAccionAuditoria.AcreditacionSaldo.ToString(),
            UsuarioId = dto.UsuarioId,
            DetalleJson = $"{{\"montoCargado\": {dto.Monto}, \"nuevoTotal\": {billetera.SaldoTotal}}}",
            Fecha = DateTime.UtcNow
        });

        return new BilleteraDto
        {
            Id = billetera.Id,
            UsuarioId = billetera.UsuarioId,
            SaldoTotal = billetera.SaldoTotal,
            SaldoRetenido = billetera.SaldoRetenido,
            SaldoDisponible = billetera.SaldoDisponible
        };
    }
}