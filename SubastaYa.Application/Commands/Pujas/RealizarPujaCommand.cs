using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Pujas;

namespace SubastaYa.Application.Commands.Pujas
{
    public record RealizarPujaCommand(
        int SubastaId,
        int CompradorId,
        decimal Monto
    ) : ICommand<ResultadoPujaDto>;
}
