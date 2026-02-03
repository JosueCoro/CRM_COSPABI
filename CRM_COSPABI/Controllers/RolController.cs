using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _rolService.ListarAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var rol = await _rolService.ObtenerPorIdAsync(id);
            if (rol == null) return NotFound();
            return Ok(rol);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Rol rol)
            => Ok(await _rolService.CrearAsync(rol));

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Rol rol)
        {
            var actualizado = await _rolService.ActualizarAsync(id, rol);
            if (actualizado == null) return NotFound();
            return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _rolService.EliminarAsync(id));
    }
}
