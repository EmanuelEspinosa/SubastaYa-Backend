using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries.Subastas
{
    public class ObtenerSubastaPorIdQueryHandler : IQueryHandler<ObtenerSubastaPorIdQuery, SubastaDto>
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IPujaRepository _pujaRepository;

        public ObtenerSubastaPorIdQueryHandler(ISubastaRepository subastaRepository, IPujaRepository pujaRepository)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
        }

        public async Task<SubastaDto> HandleAsync(ObtenerSubastaPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var subasta = await _subastaRepository.ObtenerPorIdAsync(query.Id);
            if (subasta == null)
                throw new SubastaNoEncontradaException(query.Id);

            var pujaAlta = await _pujaRepository.ObtenerPujaMasAltaAsync(subasta.Id);
            var pujas = await _pujaRepository.ObtenerPorSubastaIdAsync(subasta.Id);

            return new SubastaDto
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
            };
        }
    }
}
