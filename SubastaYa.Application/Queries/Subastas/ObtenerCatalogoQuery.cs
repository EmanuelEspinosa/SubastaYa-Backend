using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.Queries.Subastas
{
    public record ObtenerCatalogoQuery(
        int? VendedorId = null,
        EstadoSubasta? Estado = null,
        int? CategoriaId = null,
        int? CompradorId = null,
        string? Busqueda = null,
        string OrdenarPor = "fechaFin",   // fechaFin | oferta | precioBase | fechaInicio
        string Orden = "asc",             // asc | desc
        int Pagina = 1,
        int TamanoPagina = 6
    ) : IQuery<ResultadoPaginadoDto<SubastaDto>>;
}