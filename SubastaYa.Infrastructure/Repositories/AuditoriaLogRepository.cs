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
    public class AuditoriaLogRepository : IAuditoriaLogRepository
    {
        private readonly SubastaYaDbContext _context;

        public AuditoriaLogRepository(SubastaYaDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(AuditoriaLog log)
        {
            await _context.AuditoriaLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
