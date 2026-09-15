using SubastaYa.Application.DTOs.Auth;
using SubastaYa.Application.Interfaces;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SubastaYa.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegistroDto dto)
        {
            var usuarioExistente = await _usuarioRepository.ObtenerPorEmailAsync(dto.Email);
            if (usuarioExistente != null)
                return null;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Instancia de Usuario con Billetera inicializada en $0
            var nuevoUsuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = passwordHash,
                FechaRegistro = DateTime.UtcNow,
                Billetera = new Billetera
                {
                    SaldoTotal = 0m,
                    SaldoRetenido = 0m,
                    SaldoDisponible = 0m
                }
            };

            await _usuarioRepository.AgregarAsync(nuevoUsuario);

            string token = GenerarJwtToken(nuevoUsuario);

            return new AuthResponseDto
            {
                UsuarioId = nuevoUsuario.Id,
                Nombre = nuevoUsuario.Nombre,
                Email = nuevoUsuario.Email,
                Token = token
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.ObtenerPorEmailAsync(dto.Email);
            if (usuario == null) return null;

            bool passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
            if (!passwordValida) return null;

            string token = GenerarJwtToken(usuario);

            return new AuthResponseDto
            {
                UsuarioId = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Token = token
            };
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "ClaveSecretaUltraSeguraParaSubastaYa2026!");

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"] ?? "SubastaYaAPI",
                Audience = jwtSettings["Audience"] ?? "SubastaYaClient"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
