using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Billetera;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries.Billetera
{
    public class ObtenerMovimientosPorUsuarioIdQueryHandler : IQueryHandler<ObtenerMovimientosPorUsuarioIdQuery, IEnumerable<TransaccionLedgerDto>>
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;

        public ObtenerMovimientosPorUsuarioIdQueryHandler(
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository)
        {
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
        }

        public async Task<IEnumerable<TransaccionLedgerDto>> HandleAsync(ObtenerMovimientosPorUsuarioIdQuery query, CancellationToken cancellationToken = default)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioIdAsync(query.UsuarioId);
            if (billetera == null)
                return Enumerable.Empty<TransaccionLedgerDto>();

            var movimientos = await _ledgerRepository.ObtenerPorBilleteraIdAsync(billetera.Id);

            return movimientos.Select(m => new TransaccionLedgerDto
            {
                Id = m.Id,
                BilleteraId = m.BilleteraId,
                Tipo = m.Tipo,
                Monto = m.Monto,
                Fecha = m.Fecha,
                SubastaId = m.SubastaId
            });
        }
    }
}
