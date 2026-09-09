using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Interfaces
{
    public interface IPujaRepository
    {
        Task AgregarAsync(Puja puja);
        // Para mostrar el historial en la sala en vivo:
        Task<IEnumerable<Puja>> ObtenerPorSubastaIdAsync(int subastaId);
        Task<Puja?> ObtenerPujaMasAltaAsync(int subastaId);
    }
}
