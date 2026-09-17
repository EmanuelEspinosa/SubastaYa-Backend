using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.DTOs.Billetera;

public class TransaccionLedgerDto
{
    public int Id { get; set; }
    public int BilleteraId { get; set; }
    public TipoTransaccionLedger Tipo { get; set; }
    public string TipoNombre => Tipo.ToString();
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public int? SubastaId { get; set; }
}