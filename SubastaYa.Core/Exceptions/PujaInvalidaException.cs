namespace SubastaYa.Domain.Exceptions;

public class PujaInvalidaException : DomainException
{
    public PujaInvalidaException(string detalle) : base(detalle) { }
}