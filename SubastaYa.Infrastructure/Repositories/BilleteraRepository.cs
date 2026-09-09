using Microsoft.EntityFrameworkCore;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Interfaces;
using SubastaYa.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Infrastructure.Repositories
{
    public class BilleteraRepository : IBilleteraRepository
    {
        private readonly SubastaYaDbContext _context;

        public BilleteraRepository(SubastaYaDbContext context)
        {
            _context = context;
        }

        public async Task<Billetera?> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.Billeteras
                .FirstOrDefaultAsync(b => b.UsuarioId == usuarioId);
        }

        public async Task ActualizarAsync(Billetera billetera)
        {
            _context.Billeteras.Update(billetera);
            await _context.SaveChangesAsync();
        }
    }
}
