using CRM_COSPABI.DTOs;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize] // Require Login for all
    public class LecturaController : ControllerBase
    {
        private readonly ILecturaService _lecturaService;

        public LecturaController(ILecturaService lecturaService)
        {
            _lecturaService = lecturaService;
        }

        // Helper to get current User ID
        private int ObtenerUsuarioId()
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out int id))
            {
                return id;
            }
            throw new UnauthorizedAccessException("ID de usuario no identificado en el token.");
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SUPER ADMIN,ADMIN,LECTOR")] // Only staff can register readings
        public async Task<IActionResult> RegistrarLectura([FromBody] LecturaCreateDto lecturaDto)
        {
            try
            {
                int usuarioAdminId = ObtenerUsuarioId();
                var resultado = await _lecturaService.RegistrarLecturaAsync(lecturaDto, usuarioAdminId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> ObtenerLecturasPorCliente(int idCliente)
        {
            // Security Check: If user is client, can only see their own readings
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userId = ObtenerUsuarioId();

            if (roleClaim != null && (roleClaim.Contains("CLIENTE") || roleClaim.Contains("SOCIO")))
            {
                if (userId != idCliente)
                {
                     return Forbid(); // Client trying to see other's data
                }
            }

            var historial = await _lecturaService.ObtenerLecturasPorClienteAsync(idCliente);
            return Ok(historial);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerLectura(int id)
        {
            var lectura = await _lecturaService.ObtenerLecturaAsync(id);
            if (lectura == null) return NotFound();
            return Ok(lectura);
        }
    }
}
