namespace SubastaYa.Core.Domain.Entities;

public class Billetera
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public decimal SaldoTotal { get; set; }
    public decimal SaldoRetenido { get; set; }
    public decimal SaldoDisponible { get; set; }
    public int Version { get; set; } = 1; // Optimistic locking

    public Usuario Usuario { get; set; } = null!;
    public ICollection<TransaccionLedger> Movimientos { get; set; } = new List<TransaccionLedger>();
}