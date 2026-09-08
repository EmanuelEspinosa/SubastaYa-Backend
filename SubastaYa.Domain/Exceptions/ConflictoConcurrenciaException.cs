namespace SubastaYa.Domain.Exceptions;

public class ConflictoConcurrenciaException : DomainException
{
    public ConflictoConcurrenciaException(string mensaje = "El recurso fue modificado concurrentemente por otra transacción. Reintente.")
        : base(mensaje) { }
}