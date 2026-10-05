using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Billetera;

namespace SubastaYa.Application.Commands.Billetera
{
    public record CargarSaldoCommand(
        int UsuarioId,
        decimal Monto
    ) : ICommand<BilleteraDto>;
}
