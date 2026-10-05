using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Pujas;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries.Pujas
{
    public class ObtenerHistorialPujasQueryHandler : IQueryHandler<ObtenerHistorialPujasQuery, IEnumerable<PujaDto>>
    {
        private readonly IPujaRepository _pujaRepository;

        public ObtenerHistorialPujasQueryHandler(IPujaRepository pujaRepository)
        {
            _pujaRepository = pujaRepository;
        }

        public async Task<IEnumerable<PujaDto>> HandleAsync(ObtenerHistorialPujasQuery query, CancellationToken cancellationToken = default)
        {
            var pujas = await _pujaRepository.ObtenerPorSubastaIdAsync(query.SubastaId);

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
}
