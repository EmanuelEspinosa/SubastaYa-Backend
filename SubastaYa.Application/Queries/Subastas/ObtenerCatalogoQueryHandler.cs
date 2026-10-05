using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries.Subastas
{
    public class ObtenerCatalogoQueryHandler : IQueryHandler<ObtenerCatalogoQuery, IEnumerable<SubastaDto>>
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IPujaRepository _pujaRepository;

        public ObtenerCatalogoQueryHandler(ISubastaRepository subastaRepository, IPujaRepository pujaRepository)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
        }

        public async Task<IEnumerable<SubastaDto>> HandleAsync(ObtenerCatalogoQuery query, CancellationToken cancellationToken = default)
        {
            var subastas = await _subastaRepository.ObtenerTodasAsync();

            if (query.VendedorId.HasValue)
                subastas = subastas.Where(s => s.VendedorId == query.VendedorId.Value);

            if (query.Estado.HasValue)
                subastas = subastas.Where(s => s.Estado == query.Estado.Value);

            if (query.CategoriaId.HasValue)
                subastas = subastas.Where(s => s.CategoriaId == query.CategoriaId.Value);

            if (query.CompradorId.HasValue)
                subastas = subastas.Where(s => s.Pujas != null && s.Pujas.Any(p => p.CompradorId == query.CompradorId.Value));

            var resultado = new List<SubastaDto>();
            foreach (var subasta in subastas)
            {
                var pujaAlta = await _pujaRepository.ObtenerPujaMasAltaAsync(subasta.Id);
                var pujas = await _pujaRepository.ObtenerPorSubastaIdAsync(subasta.Id);

                resultado.Add(new SubastaDto
                {
                    Id = subasta.Id,
                    VendedorId = subasta.VendedorId,
                    VendedorNombre = subasta.Vendedor?.Nombre ?? string.Empty,
                    CategoriaId = subasta.CategoriaId,
                    CategoriaNombre = subasta.Categoria?.Nombre ?? string.Empty,
                    Titulo = subasta.Titulo,
                    Descripcion = subasta.Descripcion,
                    UrlImagen = subasta.UrlImagen,
                    PrecioBase = subasta.PrecioBase,
                    IncrementoMinimo = subasta.IncrementoMinimo,
                    CompradorLiderId = pujaAlta?.CompradorId,
                    OfertaMasAltaActual = pujaAlta?.Monto ?? subasta.PrecioBase,
                    CantidadOfertas = pujas.Count(),
                    FechaInicio = subasta.FechaInicio,
                    FechaFin = subasta.FechaFin,
                    Estado = subasta.Estado,
                    Version = subasta.Version
                });
            }

            return resultado;
        }
    }
}
