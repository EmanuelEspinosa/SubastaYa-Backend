using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Application.Interfaces;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Services;

public class SubastaService : ISubastaService
{
    private readonly ISubastaRepository _subastaRepository;
    private readonly IPujaRepository _pujaRepository;

    public SubastaService(ISubastaRepository subastaRepository, IPujaRepository pujaRepository)
    {
        _subastaRepository = subastaRepository;
        _pujaRepository = pujaRepository;
    }

    public async Task<IEnumerable<SubastaDto>> ObtenerCatalogoAsync(int? vendedorId = null ,EstadoSubasta? estado = null, int? categoriaId = null, int? compradorId = null)
    {
        var subastas = await _subastaRepository.ObtenerTodasAsync();

        if (vendedorId.HasValue)
            subastas = subastas.Where(s => s.VendedorId == vendedorId.Value);

        if (estado.HasValue)
            subastas = subastas.Where(s => s.Estado == estado.Value);

        if (categoriaId.HasValue)
            subastas = subastas.Where(s => s.CategoriaId == categoriaId.Value);

        // Nuevo filtro: Evalúa si el comprador hizo al menos una puja en la subasta
        if (compradorId.HasValue)
            subastas = subastas.Where(s => s.Pujas != null && s.Pujas.Any(p => p.CompradorId == compradorId.Value));

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

    public async Task<SubastaDto> ObtenerPorIdAsync(int id)
    {
        var subasta = await _subastaRepository.ObtenerPorIdAsync(id);
        if (subasta == null)
            throw new SubastaNoEncontradaException(id);

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
            OfertaMasAltaActual = pujaAlta?.Monto ?? subasta.PrecioBase,
            CantidadOfertas = pujas.Count(),
            FechaInicio = subasta.FechaInicio,
            FechaFin = subasta.FechaFin,
            Estado = subasta.Estado,
            Version = subasta.Version
        };
    }

    public async Task<SubastaDto> CrearSubastaAsync(CrearSubastaDto dto)
    {
        if (dto.PrecioBase <= 0)
            throw new DomainException("El precio base debe ser mayor a 0.");

        if (dto.IncrementoMinimo <= 0)
            throw new DomainException("El incremento mínimo debe ser mayor a 0.");

        if (dto.FechaFin <= dto.FechaInicio)
            throw new DomainException("La fecha de finalización debe ser posterior a la de inicio.");

        var ahora = DateTime.UtcNow;
        var estadoInicial = dto.FechaInicio <= ahora ? EstadoSubasta.Activa : EstadoSubasta.Programada;

        var subasta = new Subasta
        {
            VendedorId = dto.VendedorId,
            CategoriaId = dto.CategoriaId,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            UrlImagen = dto.UrlImagen,
            PrecioBase = dto.PrecioBase,
            IncrementoMinimo = dto.IncrementoMinimo,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            Estado = estadoInicial,
            Version = 1
        };

        await _subastaRepository.AgregarAsync(subasta);

        return await ObtenerPorIdAsync(subasta.Id);
    }
}