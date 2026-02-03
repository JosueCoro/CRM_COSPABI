using CRM_COSPABI.DTOs;
using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CRM_COSPABI.Service
{
    public class AuthService : IAuthService
    {
        private readonly CospabicrmContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(CospabicrmContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAdminAsync(LoginDto loginDto)
        {
            // Buscar Usuario Admin
            var usuario = await _context.UsuarioAdmins
                .Include(u => u.RolIdRolNavigation)
                    .ThenInclude(r => r.RolPermisos)
                        .ThenInclude(rp => rp.PermisoIdPermisoNavigation)
                .FirstOrDefaultAsync(u => u.Usuario == loginDto.Usuario && u.Estado == true);

            // Validar existencia y contraseña (TODO: Usar hashing en producción)
            if (usuario == null || usuario.Contraseña != loginDto.Contraseña)
            {
                return null;
            }

            // Obtener Permisos
            var permisos = usuario.RolIdRolNavigation.RolPermisos
                .Where(rp => rp.PermisoIdPermisoNavigation.Codigo != null)
                .Select(rp => rp.PermisoIdPermisoNavigation.Codigo!)
                .ToList();

            // Generar Token
            var token = GenerarToken(
                usuario.IdUsuarioAdmin.ToString(),
                usuario.Usuario,
                usuario.RolIdRolNavigation.NombreRol,
                permisos);

            return new LoginResponseDto
            {
                Token = token,
                Usuario = usuario.Usuario,
                Rol = usuario.RolIdRolNavigation.NombreRol,
                Permisos = permisos
            };
        }

        public async Task<LoginResponseDto?> LoginClienteAsync(LoginDto loginDto)
        {
            // Buscar Cuenta Cliente
            var cuenta = await _context.CuentaClientes
                .Include(c => c.ClienteIdClienteNavigation)
                    .ThenInclude(cli => cli.RolIdRolNavigation)
                        .ThenInclude(r => r.RolPermisos)
                            .ThenInclude(rp => rp.PermisoIdPermisoNavigation)
                .FirstOrDefaultAsync(c => c.Usuario == loginDto.Usuario && c.Estado == true);

            // Validar existencia y contraseña
            if (cuenta == null || cuenta.Contraseña != loginDto.Contraseña)
            {
                return null;
            }

            // El usuario real es el Cliente asociado
            var cliente = cuenta.ClienteIdClienteNavigation;
            
            // Obtener Permisos del Rol del Cliente
            var permisos = cliente.RolIdRolNavigation.RolPermisos
                .Where(rp => rp.PermisoIdPermisoNavigation.Codigo != null)
                .Select(rp => rp.PermisoIdPermisoNavigation.Codigo!)
                .ToList();

            // Generar Token
            var token = GenerarToken(
                cliente.IdCliente.ToString(),
                cuenta.Usuario,
                cliente.RolIdRolNavigation.NombreRol,
                permisos);

            return new LoginResponseDto
            {
                Token = token,
                Usuario = cuenta.Usuario,
                Rol = cliente.RolIdRolNavigation.NombreRol,
                Permisos = permisos
            };
        }

        private string GenerarToken(string id, string usuario, string rol, List<string> permisos)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, rol),
                new Claim("Permisos", JsonSerializer.Serialize(permisos)) // Custom claim para permisos
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8), // Token valido por 8 horas
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
