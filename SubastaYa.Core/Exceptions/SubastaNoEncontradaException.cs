namespace SubastaYa.Domain.Exceptions;

public class SubastaNoEncontradaException : DomainException
{
    public SubastaNoEncontradaException(int subastaId)
        : base($"No se encontró la subasta con ID {subastaId}.") { }
}