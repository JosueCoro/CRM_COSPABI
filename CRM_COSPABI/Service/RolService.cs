using CRM_COSPABI.Service.Interfaces;
using CRM_COSPABI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Service
{
    public class RolService : IRolService
    {
        private readonly CospabicrmContext _context;

        public RolService(CospabicrmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> ListarAsync()
        {
            return await _context.Rols.ToListAsync();
        }

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            return await _context.Rols.FindAsync(id);
        }

        public async Task<Rol> CrearAsync(Rol rol)
        {
            _context.Rols.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<Rol?> ActualizarAsync(int id, Rol rol)
        {
            var existente = await _context.Rols.FindAsync(id);
            if (existente == null) return null;

            existente.NombreRol = rol.NombreRol;
            existente.Estado = rol.Estado;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var rol = await _context.Rols.FindAsync(id);
            if (rol == null) return false;

            _context.Rols.Remove(rol);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
