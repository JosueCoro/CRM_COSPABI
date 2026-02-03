using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Service
{
    public class ClienteService : IClienteService
    {
        private readonly CospabicrmContext _context;

        public ClienteService(CospabicrmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> ListarAsync()
        {
            return await _context.Clientes
                .Include(c => c.RolIdRolNavigation) // Incluir información del Rol si es necesario
                .ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.RolIdRolNavigation)
                .FirstOrDefaultAsync(c => c.IdCliente == id);
        }

        public async Task<Cliente> CrearAsync(Cliente clienteInput)
        {
            var cliente = new Cliente
            {
                CodigoFijo = clienteInput.CodigoFijo,
                CodigoUbicacion = clienteInput.CodigoUbicacion,
                NombreCompleto = clienteInput.NombreCompleto,
                Ci = clienteInput.Ci,
                Direccion = clienteInput.Direccion,
                Categoria = clienteInput.Categoria,
                Actividad = clienteInput.Actividad,
                FechaRegistro = DateOnly.FromDateTime(DateTime.Now), // Asignamos fecha actual
                Estado = true, // Activo por defecto
                RolIdRol = clienteInput.RolIdRol
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> ActualizarAsync(int id, Cliente clienteInput)
        {
            var existente = await _context.Clientes.FindAsync(id);
            if (existente == null) return null;

            // Actualizamos campos permitidos
            existente.CodigoFijo = clienteInput.CodigoFijo;
            existente.CodigoUbicacion = clienteInput.CodigoUbicacion;
            existente.NombreCompleto = clienteInput.NombreCompleto;
            existente.Ci = clienteInput.Ci;
            existente.Direccion = clienteInput.Direccion;
            existente.Categoria = clienteInput.Categoria;
            existente.Actividad = clienteInput.Actividad;
            existente.Estado = clienteInput.Estado;
            existente.RolIdRol = clienteInput.RolIdRol;
            
            // Nota: No actualizamos FechaRegistro

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
