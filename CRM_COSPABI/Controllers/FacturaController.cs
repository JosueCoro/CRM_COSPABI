using CRM_COSPABI.DTOs;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _service;

        public FacturaController(IFacturaService service)
        {
            _service = service;
        }

        [HttpPost("generar")]
        public async Task<IActionResult> Generar(GenerarFacturaDto dto)
        {
            try
            {
                var factura = await _service.GenerarFacturaAsync(dto);
                return CreatedAtAction(nameof(Obtener), new { id = factura.IdFactura }, factura);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var factura = await _service.ObtenerPorIdAsync(id);
            if (factura == null) return NotFound();
            return Ok(factura);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _service.ListarAsync());
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> ListarPorCliente(int idCliente)
        {
            return Ok(await _service.ListarPorClienteAsync(idCliente));
        }

        [HttpPost("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            var result = await _service.AnularFacturaAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Factura anulada correctamente" });
        }
    }
}
