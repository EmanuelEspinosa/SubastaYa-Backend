using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.Interfaces;

public interface ISubastaService
{
    Task<IEnumerable<SubastaDto>> ObtenerCatalogoAsync(int? vendedorId = null, EstadoSubasta ? estado = null, int? categoriaId = null, int? compradorId = null);
    Task<SubastaDto> ObtenerPorIdAsync(int id);
    Task<SubastaDto> CrearSubastaAsync(CrearSubastaDto dto);
}