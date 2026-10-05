using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.Commands.Pujas;
using SubastaYa.Application.DTOs.Pujas;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Queries.Pujas;
using SubastaYa.Domain.Enums;

namespace SubastaYa.API.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly ISubastaService _subastaService;
    private readonly IPujaService _pujaService;

    public AuctionsController(ISubastaService subastaService, IPujaService pujaService)
    {
        _subastaService = subastaService;
        _pujaService = pujaService;
    }

    // GET /api/auctions (Catálogo con filtros)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubastaDto>>> ObtenerCatalogo(
        [FromQuery] int? vendedorId,
        [FromQuery] EstadoSubasta? estado,
        [FromQuery] int? categoriaId,
        [FromQuery] int? compradorId)
    {
        var subastas = await _subastaService.ObtenerCatalogoAsync(vendedorId,estado, categoriaId, compradorId);
        return Ok(subastas);
    }

    // GET /api/auctions/{id} (Detalle de subasta)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubastaDto>> ObtenerPorId(int id)
    {
        var subasta = await _subastaService.ObtenerPorIdAsync(id);
        return Ok(subasta);
    }

    // POST /api/auctions (Publicación de subasta)
    [HttpPost]
    public async Task<ActionResult<SubastaDto>> CrearSubasta([FromBody] CrearSubastaDto dto)
    {
        var nuevaSubasta = await _subastaService.CrearSubastaAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaSubasta.Id }, nuevaSubasta);
    }

    // POST /api/auctions/{id}/bids (Realizar puja con Escrow y Anti-Sniping)
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

    // GET /api/auctions/{id}/bids (Historial para la sala en vivo)
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