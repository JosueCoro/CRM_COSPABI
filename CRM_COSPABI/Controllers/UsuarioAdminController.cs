using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioAdminController : ControllerBase
    {
        private readonly IUsuarioAdminService _service;

        public UsuarioAdminController(IUsuarioAdminService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _service.ListarAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await _service.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioAdmin usuario)
        {
            var creado = await _service.CrearAsync(usuario);
            return CreatedAtAction(nameof(Obtener), new { id = creado.IdUsuarioAdmin }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, UsuarioAdmin usuario)
        {
            var actualizado = await _service.ActualizarAsync(id, usuario);
            if (actualizado == null) return NotFound();
            return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _service.EliminarAsync(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
