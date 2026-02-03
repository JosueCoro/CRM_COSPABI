using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
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
            var cliente = await _service.ObtenerPorIdAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            var creado = await _service.CrearAsync(cliente);
            return CreatedAtAction(nameof(Obtener), new { id = creado.IdCliente }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, Cliente cliente)
        {
            var actualizado = await _service.ActualizarAsync(id, cliente);
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
