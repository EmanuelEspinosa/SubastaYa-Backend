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
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly SubastaYaDbContext _context;

        public CategoriaRepository(SubastaYaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodasAsync()
        {
            return await _context.Categorias.ToListAsync();
        }
    }
}
