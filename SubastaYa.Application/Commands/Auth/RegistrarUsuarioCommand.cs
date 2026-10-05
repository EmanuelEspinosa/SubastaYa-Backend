using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Auth;

namespace SubastaYa.Application.Commands.Auth
{
    public record RegistrarUsuarioCommand(
        string Nombre,
        string Email,
        string Password
    ) : ICommand<AuthResponseDto?>;
}
