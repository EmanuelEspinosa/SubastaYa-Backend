using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.DTOs.Actividades;

/// <summary>Actividad del usuario como vendedor (una fila por subasta publicada).</summary>
public record ActividadPublicacionDto(
    int SubastaId, string Titulo, string UrlImagen, decimal PrecioBase,
    decimal OfertaActual, EstadoSubasta Estado, int CantidadPujas,
    int? CompradorLiderId, DateTime FechaFin);