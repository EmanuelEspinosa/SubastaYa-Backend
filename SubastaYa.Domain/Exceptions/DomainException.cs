namespace SubastaYa.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string mensaje) : base(mensaje) { }
}