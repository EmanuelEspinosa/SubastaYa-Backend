namespace SubastaYa.Domain.Exceptions;

public class SaldoInsuficienteException : DomainException
{
    public SaldoInsuficienteException(decimal disponible, decimal requerido)
        : base($"Saldo insuficiente. Disponible: ${disponible:N2}, Requerido: ${requerido:N2}.") { }
}