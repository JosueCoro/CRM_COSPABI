using CRM_COSPABI.DTOs;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface INotificacionService
    {
        Task<IEnumerable<NotificacionDto>> ListarAsync();
        Task<NotificacionDto?> ObtenerPorIdAsync(int id);
        Task<NotificacionDto> CrearAsync(CreateNotificacionDto dto);
        Task<NotificacionDto?> ActualizarAsync(int id, UpdateNotificacionDto dto);
        Task<bool> EliminarAsync(int id);
    }
}
