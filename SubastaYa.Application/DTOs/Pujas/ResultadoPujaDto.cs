namespace SubastaYa.Application.DTOs.Pujas;

public class ResultadoPujaDto
{
    public bool Exitosa { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public decimal MontoOfertado { get; set; }
    public bool TiempoExtendido { get; set; }
    public DateTime NuevaFechaFin { get; set; }
}