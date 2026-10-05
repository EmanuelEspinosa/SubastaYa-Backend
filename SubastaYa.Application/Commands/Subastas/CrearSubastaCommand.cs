using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;

namespace SubastaYa.Application.Commands.Subastas
{
    public record CrearSubastaCommand(
        int VendedorId,
        int CategoriaId,
        string Titulo,
        string Descripcion,
        string UrlImagen,
        decimal PrecioBase,
        decimal IncrementoMinimo,
        DateTime FechaInicio,
        DateTime FechaFin
    ) : ICommand<SubastaDto>;
}
