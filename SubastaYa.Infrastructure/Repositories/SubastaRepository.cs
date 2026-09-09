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
    public class SubastaRepository : ISubastaRepository
    {
        private readonly SubastaYaDbContext _context;

        public SubastaRepository(SubastaYaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subasta>> ObtenerTodasAsync()
        {
            // Traemos las subastas incluyendo la información de la categoría y el vendedor
            return await _context.Subastas
                .Include(s => s.Categoria)
                .Include(s => s.Vendedor)
                .ToListAsync();
        }

        public async Task<Subasta?> ObtenerPorIdAsync(int id)
        {
            // Para la sala en vivo, necesitamos todo el detalle, incluidas las pujas previas
            return await _context.Subastas
                .Include(s => s.Categoria)
                .Include(s => s.Vendedor)
                .Include(s => s.Pujas)
                    .ThenInclude(p => p.Comprador)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AgregarAsync(Subasta subasta)
        {
            await _context.Subastas.AddAsync(subasta);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Subasta subasta)
        {
            _context.Subastas.Update(subasta);
            await _context.SaveChangesAsync();
        }
    }
}
