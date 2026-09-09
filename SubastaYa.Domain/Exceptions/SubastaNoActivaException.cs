namespace SubastaYa.Domain.Exceptions;

public class SubastaNoActivaException : DomainException
{
    public SubastaNoActivaException(string estadoActual)
        : base($"No se pueden realizar ofertas en una subasta con estado '{estadoActual}'.") { }
}