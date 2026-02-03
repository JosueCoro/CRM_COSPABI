using CRM_COSPABI.DTOs;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _service;

        public NotificacionController(INotificacionService service)
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
            var notificacion = await _service.ObtenerPorIdAsync(id);
            if (notificacion == null) return NotFound();
            return Ok(notificacion);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CreateNotificacionDto dto)
        {
            var created = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(Obtener), new { id = created.IdNotificacion }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, UpdateNotificacionDto dto)
        {
            var updated = await _service.ActualizarAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var deleted = await _service.EliminarAsync(id);
            if (!deleted) return NotFound();
            return NoContent(); // 204 No Content es standard para delete exitoso
        }
    }
}
