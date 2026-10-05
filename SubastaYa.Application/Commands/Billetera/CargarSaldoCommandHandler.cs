using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Billetera;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;
using System.Transactions;

namespace SubastaYa.Application.Commands.Billetera
{
    public class CargarSaldoCommandHandler : ICommandHandler<CargarSaldoCommand, BilleteraDto>
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;
        private readonly IAuditoriaLogRepository _auditoriaRepository;

        public CargarSaldoCommandHandler(
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository,
            IAuditoriaLogRepository auditoriaRepository)
        {
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<BilleteraDto> HandleAsync(CargarSaldoCommand command, CancellationToken cancellationToken = default)
        {
            if (command.Monto <= 0)
                throw new DomainException("El monto a cargar debe ser mayor a cero.");

            var billetera = await _billeteraRepository.ObtenerPorUsuarioIdAsync(command.UsuarioId);
            if (billetera == null)
                throw new DomainException($"No se encontró la billetera del usuario {command.UsuarioId}.");

            var ahora = DateTime.UtcNow;

            // =========================================================================
            // BLOQUE TRANSACCIONAL ATÓMICO (Cumple con requerimiento B07)
            // =========================================================================

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Actualizar saldos
            billetera.SaldoTotal += command.Monto;
            billetera.SaldoDisponible += command.Monto;
            billetera.Version++;

            await _billeteraRepository.ActualizarAsync(billetera);

            // Registrar en el Libro Mayor (Ledger)
            await _ledgerRepository.AgregarAsync(new TransaccionLedger
            {
                BilleteraId = billetera.Id,
                Tipo = TipoTransaccionLedger.Deposito,
                Monto = command.Monto,
                Fecha = DateTime.UtcNow
            });

            // Registrar Auditoría obligatoria
            await _auditoriaRepository.AgregarAsync(new AuditoriaLog
            {
                Entidad = "BILLETERA",
                EntidadId = billetera.Id,
                Accion = TipoAccionAuditoria.AcreditacionSaldo.ToString(),
                UsuarioId = command.UsuarioId,
                DetalleJson = $"{{\"montoCargado\": {command.Monto}, \"nuevoTotal\": {billetera.SaldoTotal}}}",
                Fecha = ahora
            });

            // Confirmar transacción atómica completa (All-or-Nothing)
            scope.Complete();

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
}
