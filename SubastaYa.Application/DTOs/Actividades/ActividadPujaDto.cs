using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.DTOs.Actividades;

/// <summary>Actividad del usuario como postor (una fila por subasta donde pujó).</summary>
public record ActividadPujaDto(
    int SubastaId, string Titulo, string UrlImagen, decimal PrecioBase,
    decimal OfertaActual, EstadoSubasta Estado, decimal MiUltimaPuja,
    int MisPujas, bool SoyLider, string Resultado, DateTime FechaFin);