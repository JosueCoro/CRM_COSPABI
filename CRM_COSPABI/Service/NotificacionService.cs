using CRM_COSPABI.DTOs;
using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Service
{
    public class NotificacionService : INotificacionService
    {
        private readonly CospabicrmContext _context;

        public NotificacionService(CospabicrmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificacionDto>> ListarAsync()
        {
            var list = await _context.Notificacions
                .OrderByDescending(n => n.FechaPublicacion)
                .ToListAsync();
            
            return list.Select(n => MapToDto(n));
        }

        public async Task<NotificacionDto?> ObtenerPorIdAsync(int id)
        {
            var notificacion = await _context.Notificacions.FindAsync(id);
            return notificacion == null ? null : MapToDto(notificacion);
        }

        public async Task<NotificacionDto> CrearAsync(CreateNotificacionDto dto)
        {
            var notificacion = new Notificacion
            {
                Titulo = dto.Titulo,
                Mensaje = dto.Mensaje,
                Tipo = dto.Tipo,
                FechaPublicacion = DateOnly.FromDateTime(DateTime.Now),
                Estado = true // Activo por defecto
            };

            _context.Notificacions.Add(notificacion);
            await _context.SaveChangesAsync();

            return MapToDto(notificacion);
        }

        public async Task<NotificacionDto?> ActualizarAsync(int id, UpdateNotificacionDto dto)
        {
            var existing = await _context.Notificacions.FindAsync(id);
            if (existing == null) return null;

            existing.Titulo = dto.Titulo;
            existing.Mensaje = dto.Mensaje;
            existing.Tipo = dto.Tipo;
            existing.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return MapToDto(existing);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var existing = await _context.Notificacions.FindAsync(id);
            if (existing == null) return false;

            _context.Notificacions.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        private static NotificacionDto MapToDto(Notificacion n)
        {
            return new NotificacionDto
            {
                IdNotificacion = n.IdNotificacion,
                Titulo = n.Titulo,
                Mensaje = n.Mensaje,
                Tipo = n.Tipo,
                FechaPublicacion = n.FechaPublicacion,
                Estado = n.Estado
            };
        }
    }
}
