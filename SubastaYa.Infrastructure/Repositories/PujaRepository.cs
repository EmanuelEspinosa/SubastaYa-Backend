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
    public class PujaRepository : IPujaRepository
    {
        private readonly SubastaYaDbContext _context;

        public PujaRepository(SubastaYaDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(Puja puja)
        {
            await _context.Pujas.AddAsync(puja);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Puja>> ObtenerPorSubastaIdAsync(int subastaId)
        {
            // Trae las pujas de una subasta específica, ordenadas de la más reciente a la más antigua
            return await _context.Pujas
                .Include(p => p.Comprador)
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.FechaPuja)
                .ToListAsync();
        }

        public async Task<Puja?> ObtenerPujaMasAltaAsync(int subastaId)
        {
            // Trae solo una puja (la del monto más alto) para saber quién va ganando
            return await _context.Pujas
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.Monto)
                .FirstOrDefaultAsync();
        }
    }
}
