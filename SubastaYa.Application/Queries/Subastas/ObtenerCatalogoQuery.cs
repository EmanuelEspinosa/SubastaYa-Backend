using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Enums;

namespace SubastaYa.Application.Queries.Subastas
{
    public record ObtenerCatalogoQuery(
        int? VendedorId = null,
        EstadoSubasta? Estado = null,
        int? CategoriaId = null,
        int? CompradorId = null
    ) : IQuery<IEnumerable<SubastaDto>>;
}
