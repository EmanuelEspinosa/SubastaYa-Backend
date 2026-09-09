using SubastaYa.Application.DTOs.Billetera;

namespace SubastaYa.Application.Interfaces;

public interface IBilleteraService
{
    Task<BilleteraDto> ObtenerSaldoPorUsuarioIdAsync(int usuarioId);
    Task<BilleteraDto> CargarSaldoAsync(CargarSaldoDto dto);
}