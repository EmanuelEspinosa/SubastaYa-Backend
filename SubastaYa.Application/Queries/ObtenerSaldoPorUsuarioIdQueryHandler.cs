using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Billetera;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries
{
    public class ObtenerSaldoPorUsuarioIdQueryHandler : IQueryHandler<ObtenerSaldoPorUsuarioIdQuery, BilleteraDto>
    {
        private readonly IBilleteraRepository _billeteraRepository;

        public ObtenerSaldoPorUsuarioIdQueryHandler(IBilleteraRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<BilleteraDto> HandleAsync(ObtenerSaldoPorUsuarioIdQuery query, CancellationToken cancellationToken = default)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioIdAsync(query.UsuarioId);
            if (billetera == null)
                throw new DomainException($"No se encontró la billetera del usuario {query.UsuarioId}.");

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
