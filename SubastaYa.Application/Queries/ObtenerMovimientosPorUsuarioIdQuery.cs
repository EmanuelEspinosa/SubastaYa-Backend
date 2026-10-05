using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Billetera;

namespace SubastaYa.Application.Queries
{
    public record ObtenerMovimientosPorUsuarioIdQuery(int UsuarioId) : IQuery<IEnumerable<TransaccionLedgerDto>>;
}
