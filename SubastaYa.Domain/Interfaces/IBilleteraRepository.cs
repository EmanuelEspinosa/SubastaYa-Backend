using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Interfaces
{
    internal interface IBilleteraRepository
    {
        Task<Billetera?> ObtenerPorUsuarioIdAsync(int usuarioId);

        // Fundamental para el bloque transaccional atómico
        Task ActualizarAsync(Billetera billetera);
    }
}
