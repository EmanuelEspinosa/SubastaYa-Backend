using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.DTOs.Auth;
using SubastaYa.Application.Interfaces;

namespace SubastaYa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistroDto dto)
        {
            var response = await _authService.RegisterAsync(dto);
            if (response == null)
                return BadRequest(new { mensaje = "El correo electrónico ya se encuentra registrado." });

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            if (response == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas. Verifique email y contraseña." });

            return Ok(response);
        }
    }
}
