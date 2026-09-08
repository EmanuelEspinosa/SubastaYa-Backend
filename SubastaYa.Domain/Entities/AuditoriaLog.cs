namespace SubastaYa.Domain.Entities;

public class AuditoriaLog
{
    public int Id { get; set; }
    public string Entidad { get; set; } = string.Empty;
    public int EntidadId { get; set; }
    public string Accion { get; set; } = string.Empty;
    public int? UsuarioId { get; set; } // Nullable para acciones automáticas del Worker
    public string DetalleJson { get; set; } = "{}";
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public Usuario? Usuario { get; set; }
}