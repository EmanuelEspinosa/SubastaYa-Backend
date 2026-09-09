using SubastaYa.Application.DTOs.Pujas;

namespace SubastaYa.Application.Interfaces;

public interface IPujaService
{
    Task<ResultadoPujaDto> RealizarPujaAsync(CrearPujaDto dto);
    Task<IEnumerable<PujaDto>> ObtenerHistorialPorSubastaIdAsync(int subastaId);
}