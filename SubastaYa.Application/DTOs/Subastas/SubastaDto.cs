using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.DTOs.Subastas;

public class SubastaDto
{
    public int Id { get; set; }
    public int VendedorId { get; set; }
    public string VendedorNombre { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string UrlImagen { get; set; } = string.Empty;
    public decimal PrecioBase { get; set; }
    public decimal IncrementoMinimo { get; set; }
    public decimal OfertaMasAltaActual { get; set; }
    public int CantidadOfertas { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoSubasta Estado { get; set; }
    public int Version { get; set; }
}