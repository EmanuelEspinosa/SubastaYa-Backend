using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.Commands.Pujas;
using SubastaYa.Application.Commands.Subastas;
using SubastaYa.Application.DTOs.Pujas;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Application.Queries.Pujas;
using SubastaYa.Application.Queries.Subastas;
using SubastaYa.Domain.Enums;

namespace SubastaYa.API.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    // GET /api/auctions (Catálogo)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubastaDto>>> ObtenerCatalogo(
        [FromQuery] int? vendedorId,
        [FromQuery] EstadoSubasta? estado,
        [FromQuery] int? categoriaId,
        [FromQuery] int? compradorId,
        [FromServices] IQueryHandler<ObtenerCatalogoQuery, IEnumerable<SubastaDto>> handler)
    {
        var query = new ObtenerCatalogoQuery(vendedorId, estado, categoriaId, compradorId);
        var subastas = await handler.HandleAsync(query);
        return Ok(subastas);
    }

    // GET /api/auctions/{id} (Detalle)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubastaDto>> ObtenerPorId(
        int id,
        [FromServices] IQueryHandler<ObtenerSubastaPorIdQuery, SubastaDto> handler)
    {
        var query = new ObtenerSubastaPorIdQuery(id);
        var subasta = await handler.HandleAsync(query);
        return Ok(subasta);
    }

    // POST /api/auctions (Creación)
    [HttpPost]
    public async Task<ActionResult<SubastaDto>> CrearSubasta(
        [FromBody] CrearSubastaCommand command,
        [FromServices] ICommandHandler<CrearSubastaCommand, SubastaDto> handler)
    {
        var nuevaSubasta = await handler.HandleAsync(command);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaSubasta.Id }, nuevaSubasta);
    }

    // POST /api/auctions/{id}/bids (Pujas)
    [HttpPost("{id:int}/bids")]
    public async Task<ActionResult<ResultadoPujaDto>> RealizarPuja(
        int id,
        [FromBody] CrearPujaDto dto,
        [FromServices] ICommandHandler<RealizarPujaCommand, ResultadoPujaDto> handler)
    {
        var command = new RealizarPujaCommand(id, dto.CompradorId, dto.Monto);
        var resultado = await handler.HandleAsync(command);
        return Ok(resultado);
    }

    // GET /api/auctions/{id}/bids (Historial)
    [HttpGet("{id:int}/bids")]
    public async Task<ActionResult<IEnumerable<PujaDto>>> ObtenerHistorialPujas(
        int id,
        [FromServices] IQueryHandler<ObtenerHistorialPujasQuery, IEnumerable<PujaDto>> handler)
    {
        var query = new ObtenerHistorialPujasQuery(id);
        var historial = await handler.HandleAsync(query);
        return Ok(historial);
    }
}