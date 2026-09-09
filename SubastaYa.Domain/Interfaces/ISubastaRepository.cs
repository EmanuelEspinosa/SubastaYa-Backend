using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Interfaces
{
    internal interface ISubastaRepository
    {
        // Para Catálogo y Exploración
        Task<IEnumerable<Subasta>> ObtenerTodasAsync();

        // Para Detalle y validación antes de pujar
        Task<Subasta?> ObtenerPorIdAsync(int id);

        // Para publicación de nueva subasta por el Vendedor
        Task AgregarAsync(Subasta subasta);

        // Para aplicar la regla Anti-Sniping y actualizar el estado
        Task ActualizarAsync(Subasta subasta);
    }
}
