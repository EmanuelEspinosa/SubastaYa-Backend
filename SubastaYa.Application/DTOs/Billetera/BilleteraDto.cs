namespace SubastaYa.Application.DTOs.Billetera;

public class BilleteraDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public decimal SaldoTotal { get; set; }
    public decimal SaldoRetenido { get; set; }
    public decimal SaldoDisponible { get; set; }
}