using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Commands.Subastas
{
    public class CrearSubastaCommandHandler : ICommandHandler<CrearSubastaCommand, SubastaDto>
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IPujaRepository _pujaRepository;

        public CrearSubastaCommandHandler(ISubastaRepository subastaRepository, IPujaRepository pujaRepository)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
        }

        public async Task<SubastaDto> HandleAsync(CrearSubastaCommand command, CancellationToken cancellationToken = default)
        {
            if (command.PrecioBase <= 0)
                throw new DomainException("El precio base debe ser mayor a 0.");

            if (command.IncrementoMinimo <= 0)
                throw new DomainException("El incremento mínimo debe ser mayor a 0.");

            if (command.FechaFin <= command.FechaInicio)
                throw new DomainException("La fecha de finalización debe ser posterior a la de inicio.");

            var ahora = DateTime.UtcNow;
            var estadoInicial = command.FechaInicio <= ahora ? EstadoSubasta.Activa : EstadoSubasta.Programada;

            var subasta = new Subasta
            {
                VendedorId = command.VendedorId,
                CategoriaId = command.CategoriaId,
                Titulo = command.Titulo,
                Descripcion = command.Descripcion,
                UrlImagen = command.UrlImagen,
                PrecioBase = command.PrecioBase,
                IncrementoMinimo = command.IncrementoMinimo,
                FechaInicio = command.FechaInicio.ToUniversalTime(),
                FechaFin = command.FechaFin.ToUniversalTime(),
                Estado = estadoInicial,
                Version = 1
            };

            await _subastaRepository.AgregarAsync(subasta);

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
