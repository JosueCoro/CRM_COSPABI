using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Service
{
    public class UsuarioAdminService : IUsuarioAdminService
    {
        private readonly CospabicrmContext _context;
        private readonly PasswordHasher<UsuarioAdmin> _passwordHasher = new();


        public UsuarioAdminService(CospabicrmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioAdmin>> ListarAsync()
        {
            return await _context.UsuarioAdmins.ToListAsync();
        }

        public async Task<UsuarioAdmin?> ObtenerPorIdAsync(int id)
        {
            return await _context.UsuarioAdmins.FindAsync(id);
        }

        public async Task<UsuarioAdmin> CrearAsync(UsuarioAdmin user)
        {
            var usuario = new UsuarioAdmin
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Usuario = user.Usuario,
                Cargo = user.Cargo,
                RolIdRol = user.RolIdRol,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            usuario.Contraseña = _passwordHasher.HashPassword(usuario, user.Contraseña);

            _context.UsuarioAdmins.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<UsuarioAdmin?> ActualizarAsync(int id, UsuarioAdmin usuario)
        {
            var existente = await _context.UsuarioAdmins.FindAsync(id);
            if (existente == null) return null;

            existente.Nombre = usuario.Nombre;
            existente.Apellido = usuario.Apellido;
            existente.Usuario = usuario.Usuario;
            existente.Cargo = usuario.Cargo;
            existente.Estado = usuario.Estado;
            existente.RolIdRol = usuario.RolIdRol;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _context.UsuarioAdmins.FindAsync(id);
            if (usuario == null) return false;

            _context.UsuarioAdmins.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
