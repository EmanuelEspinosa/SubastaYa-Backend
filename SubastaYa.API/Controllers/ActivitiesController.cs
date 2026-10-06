using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Actividades;
using SubastaYa.Application.Queries.Actividades;

namespace SubastaYa.API.Controllers;

/// <summary>Consultas dedicadas de actividad por usuario (Mis Pujas / Mis Publicaciones).</summary>
[ApiController]
[Route("api/users/{usuarioId:int}")]
public class ActivitiesController : ControllerBase
{
    /// <summary>Subastas donde el usuario pujó, con última oferta y resultado.</summary>
    [HttpGet("bids")]
    [ProducesResponseType(typeof(IEnumerable<ActividadPujaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ActividadPujaDto>>> ObtenerMisPujas(
        int usuarioId,
        [FromServices] IQueryHandler<ObtenerMisPujasQuery, IEnumerable<ActividadPujaDto>> handler)
        => Ok(await handler.HandleAsync(new ObtenerMisPujasQuery(usuarioId)));

    /// <summary>Subastas publicadas por el usuario con métricas de participación.</summary>
    [HttpGet("auctions")]
    [ProducesResponseType(typeof(IEnumerable<ActividadPublicacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ActividadPublicacionDto>>> ObtenerMisPublicaciones(
        int usuarioId,
        [FromServices] IQueryHandler<ObtenerMisPublicacionesQuery, IEnumerable<ActividadPublicacionDto>> handler)
        => Ok(await handler.HandleAsync(new ObtenerMisPublicacionesQuery(usuarioId)));
}