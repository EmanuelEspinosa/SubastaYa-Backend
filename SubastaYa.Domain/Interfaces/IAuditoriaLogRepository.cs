using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Interfaces
{
    public interface IAuditoriaLogRepository
    {
        Task AgregarAsync(AuditoriaLog log);
    }
}
