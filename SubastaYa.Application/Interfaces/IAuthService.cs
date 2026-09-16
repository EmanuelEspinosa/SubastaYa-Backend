using SubastaYa.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegistroDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
