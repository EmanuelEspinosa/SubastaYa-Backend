using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.Commands.Auth;
using SubastaYa.Application.DTOs.Auth;
using SubastaYa.Application.Interfaces;

namespace SubastaYa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegistrarUsuarioCommand command,
            [FromServices] ICommandHandler<RegistrarUsuarioCommand, AuthResponseDto?> handler)
        {
            var response = await handler.HandleAsync(command);
            if (response == null)
                return BadRequest(new { mensaje = "El correo electrónico ya se encuentra registrado." });

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command,
            [FromServices] ICommandHandler<LoginCommand, AuthResponseDto?> handler)
        {
            var response = await handler.HandleAsync(command);
            if (response == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas. Verifique email y contraseña." });

            return Ok(response);
        }
    }
}
