using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.DTOs.Billetera;
using SubastaYa.Application.Interfaces;

namespace SubastaYa.API.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletsController : ControllerBase
{
    private readonly IBilleteraService _billeteraService;

    public WalletsController(IBilleteraService billeteraService)
    {
        _billeteraService = billeteraService;
    }

    // GET /api/wallet/balance?usuarioId=2 (Consulta desglose de saldos)
    [HttpGet("balance")]
    public async Task<ActionResult<BilleteraDto>> ObtenerBalance([FromQuery] int usuarioId)
    {
        var balance = await _billeteraService.ObtenerSaldoPorUsuarioIdAsync(usuarioId);
        return Ok(balance);
    }

    // POST /api/wallet/deposit (Carga de saldo simulada)
    [HttpPost("deposit")]
    public async Task<ActionResult<BilleteraDto>> CargarSaldo([FromBody] CargarSaldoDto dto)
    {
        var balanceActualizado = await _billeteraService.CargarSaldoAsync(dto);
        return Ok(balanceActualizado);
    }
}