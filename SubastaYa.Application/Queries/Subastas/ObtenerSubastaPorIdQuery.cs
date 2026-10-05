using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;

namespace SubastaYa.Application.Queries.Subastas
{
    public record ObtenerSubastaPorIdQuery(int Id) : IQuery<SubastaDto>;
}
