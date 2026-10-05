using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Auth;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Interfaces;
using BCrypt.Net;


namespace SubastaYa.Application.Commands.Auth
{
    public class RegistrarUsuarioCommandHandler : ICommandHandler<RegistrarUsuarioCommand, AuthResponseDto?>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public RegistrarUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> HandleAsync(RegistrarUsuarioCommand command, CancellationToken cancellationToken = default)
        {
            var usuarioExistente = await _usuarioRepository.ObtenerPorEmailAsync(command.Email);
            if (usuarioExistente != null)
                return null;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

            var nuevoUsuario = new Usuario
            {
                Nombre = command.Nombre,
                Email = command.Email,
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
