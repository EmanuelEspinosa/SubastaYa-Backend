namespace SubastaYa.Application.DTOs.Pujas;

public class PujaDto
{
    public int Id { get; set; }
    public int SubastaId { get; set; }
    public int CompradorId { get; set; }
    public string CompradorSeudonimo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime FechaPuja { get; set; }
}