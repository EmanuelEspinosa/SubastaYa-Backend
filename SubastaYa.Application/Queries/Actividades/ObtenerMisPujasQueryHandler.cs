using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Actividades;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries.Actividades;

public class ObtenerMisPujasQueryHandler : IQueryHandler<ObtenerMisPujasQuery, IEnumerable<ActividadPujaDto>>
{
    private readonly IPujaRepository _pujaRepository;
    public ObtenerMisPujasQueryHandler(IPujaRepository pujaRepository) => _pujaRepository = pujaRepository;

    public async Task<IEnumerable<ActividadPujaDto>> HandleAsync(ObtenerMisPujasQuery query, CancellationToken cancellationToken = default)
    {
        var pujas = await _pujaRepository.ObtenerPorCompradorAsync(query.UsuarioId);
        return pujas.GroupBy(p => p.SubastaId).Select(g =>
        {
            var subasta = g.First().Subasta;
            var lider = subasta.Pujas.OrderByDescending(p => p.Monto).First();
            var miUltima = g.OrderByDescending(p => p.Monto).First();
            var soyLider = lider.CompradorId == query.UsuarioId;
            var resultado = subasta.Estado switch
            {
                EstadoSubasta.Finalizada => soyLider ? "Ganada" : "Perdida",
                EstadoSubasta.Activa => soyLider ? "Ganando" : "Superado",
                EstadoSubasta.Desierta => "Desierta",
                _ => "En espera"
            };
            return new ActividadPujaDto(subasta.Id, subasta.Titulo, subasta.UrlImagen, subasta.PrecioBase,
                lider.Monto, subasta.Estado, miUltima.Monto, g.Count(), soyLider, resultado, subasta.FechaFin);
        }).ToList();
    }
}