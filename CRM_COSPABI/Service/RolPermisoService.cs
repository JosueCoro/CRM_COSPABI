using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Service
{
    public class RolPermisoService : IRolPermisoService
    {
        private readonly CospabicrmContext _context;

        public RolPermisoService(CospabicrmContext context)
        {
            _context = context;
        }

        public async Task AsignarPermisosAsync(int rolId, List<int> permisosIds)
        {
            var existentes = _context.RolPermisos
                .Where(rp => rp.RolIdRol == rolId);

            _context.RolPermisos.RemoveRange(existentes);

            foreach (var permisoId in permisosIds)
            {
                _context.RolPermisos.Add(new RolPermiso
                {
                    RolIdRol = rolId,
                    PermisoIdPermiso = permisoId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> ObtenerPermisosPorRolAsync(int rolId)
        {
            return await _context.RolPermisos
                .Where(rp => rp.RolIdRol == rolId)
                .Select(rp => rp.PermisoIdPermiso)
                .ToListAsync();
        }

    }

}
