using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Actividades;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries.Actividades;

public class ObtenerMisPublicacionesQueryHandler : IQueryHandler<ObtenerMisPublicacionesQuery, IEnumerable<ActividadPublicacionDto>>
{
    private readonly ISubastaRepository _subastaRepository;
    public ObtenerMisPublicacionesQueryHandler(ISubastaRepository subastaRepository) => _subastaRepository = subastaRepository;

    public async Task<IEnumerable<ActividadPublicacionDto>> HandleAsync(ObtenerMisPublicacionesQuery query, CancellationToken cancellationToken = default)
    {
        var subastas = await _subastaRepository.ObtenerTodasAsync();
        return subastas.Where(s => s.VendedorId == query.UsuarioId).Select(s =>
        {
            var lider = s.Pujas.OrderByDescending(p => p.Monto).FirstOrDefault();
            return new ActividadPublicacionDto(s.Id, s.Titulo, s.UrlImagen, s.PrecioBase,
                lider?.Monto ?? s.PrecioBase, s.Estado, s.Pujas.Count, lider?.CompradorId, s.FechaFin);
        }).ToList();
    }
}