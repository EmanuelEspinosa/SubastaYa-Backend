using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.Interfaces;

public interface ISubastaService
{
    Task<IEnumerable<SubastaDto>> ObtenerCatalogoAsync(EstadoSubasta? estado = null, int? categoriaId = null);
    Task<SubastaDto> ObtenerPorIdAsync(int id);
    Task<SubastaDto> CrearSubastaAsync(CrearSubastaDto dto);
}