using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.Commands.Billetera;
using SubastaYa.Application.DTOs.Billetera;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Queries;

namespace SubastaYa.API.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletsController : ControllerBase
{
    // GET /api/wallet/balance?usuarioId=2
    [HttpGet("balance")]
    public async Task<ActionResult<BilleteraDto>> ObtenerBalance(
        [FromQuery] int usuarioId,
        [FromServices] IQueryHandler<ObtenerSaldoPorUsuarioIdQuery, BilleteraDto> handler)
    {
        var query = new ObtenerSaldoPorUsuarioIdQuery(usuarioId);
        var balance = await handler.HandleAsync(query);
        return Ok(balance);
    }

    // POST /api/wallet/deposit
    [HttpPost("deposit")]
    public async Task<ActionResult<BilleteraDto>> CargarSaldo(
        [FromBody] CargarSaldoCommand command,
        [FromServices] ICommandHandler<CargarSaldoCommand, BilleteraDto> handler)
    {
        var balanceActualizado = await handler.HandleAsync(command);
        return Ok(balanceActualizado);
    }

    // GET /api/wallet/transactions?usuarioId=2
    [HttpGet("transactions")]
    public async Task<ActionResult<IEnumerable<TransaccionLedgerDto>>> ObtenerTransacciones(
        [FromQuery] int usuarioId,
        [FromServices] IQueryHandler<ObtenerMovimientosPorUsuarioIdQuery, IEnumerable<TransaccionLedgerDto>> handler)
    {
        var query = new ObtenerMovimientosPorUsuarioIdQuery(usuarioId);
        var movimientos = await handler.HandleAsync(query);
        return Ok(movimientos);
    }
}