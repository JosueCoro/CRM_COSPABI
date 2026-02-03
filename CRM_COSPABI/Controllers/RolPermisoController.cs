using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolPermisoController : ControllerBase
    {
        private readonly IRolPermisoService _service;

        public RolPermisoController(IRolPermisoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Asignar(
            int rolId,
            [FromBody] List<int> permisosIds)
        {
            await _service.AsignarPermisosAsync(rolId, permisosIds);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int rolId)
            => Ok(await _service.ObtenerPermisosPorRolAsync(rolId));
    }
}
