using CRM_COSPABI.Models;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface IRolPermisoService
    {
        Task AsignarPermisosAsync(int rolId, List<int> permisosIds);
        Task<List<int>> ObtenerPermisosPorRolAsync(int rolId);
    }
}
