namespace SubastaYa.Application.DTOs.Subastas;

/// <summary>Contenedor genérico de paginación para consultas de lectura.</summary>
public record ResultadoPaginadoDto<T>(
    IReadOnlyList<T> Items, int Total, int Pagina, int TamanoPagina, int TotalPaginas);