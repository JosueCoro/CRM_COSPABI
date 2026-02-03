using CRM_COSPABI.Models;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> ListarAsync();
        Task<Rol?> ObtenerPorIdAsync(int id);
        Task<Rol> CrearAsync(Rol rol);
        Task<Rol?> ActualizarAsync(int id, Rol rol);
        Task<bool> EliminarAsync(int id);
    }
}
