using CRM_COSPABI.DTOs;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpPost("iniciar")]
        public async Task<IActionResult> IniciarPago([FromBody] IniciarPagoDto pagoDto)
        {
            try
            {
                var resultado = await _pagoService.IniciarPagoAsync(pagoDto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromBody] LibelulaCallbackDto callbackDto)
        {
            try
            {
                // Log payload here if needed
                var resultado = await _pagoService.ProcesarCallbackAsync(callbackDto);
                if (resultado)
                    return Ok(new { message = "Pago registrado correctamente" });
                
                return BadRequest(new { message = "Error al procesar el pago" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
