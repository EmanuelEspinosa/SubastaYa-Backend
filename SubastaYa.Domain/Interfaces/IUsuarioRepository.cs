using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task AgregarAsync(Usuario usuario);
    }
}
