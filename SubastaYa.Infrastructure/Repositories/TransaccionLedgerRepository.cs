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
    public class TransaccionLedgerRepository : ITransaccionLedgerRepository
    {
        private readonly SubastaYaDbContext _context;

        public TransaccionLedgerRepository(SubastaYaDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(TransaccionLedger transaccion)
        {
            await _context.TransaccionesLedger.AddAsync(transaccion);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransaccionLedger>> ObtenerPorBilleteraIdAsync(int billeteraId)
        {
            return await _context.TransaccionesLedger
                .Where(t => t.BilleteraId == billeteraId)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
        }
    }
}
