using CRM_COSPABI.Models;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface IUsuarioAdminService
    {
        Task<IEnumerable<UsuarioAdmin>> ListarAsync();
        Task<UsuarioAdmin?> ObtenerPorIdAsync(int id);
        Task<UsuarioAdmin> CrearAsync(UsuarioAdmin usuario);
        Task<UsuarioAdmin?> ActualizarAsync(int id, UsuarioAdmin usuario);
        Task<bool> EliminarAsync(int id);
    }
}
