using CRM_COSPABI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly CospabicrmContext _context;

        public NotificacionController(CospabicrmContext context)
        {
            _context = context;
        }

        [HttpGet("ListarNotificacion")]
        public async Task<ActionResult<IEnumerable<Notificacion>>> Listar()
        {
            var notifiicaciones = await _context.Notificacions.ToListAsync();
            return Ok(notifiicaciones);
        }

        [HttpPost("RegistrarNotificacion")]
        public async Task<ActionResult<Notificacion>> GuardarNotificacion(Notificacion notificacion)
        {
            _context.Notificacions.Add(notificacion);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, notificacion);
        }



        [HttpPut("EditarNotificacion/{id}")]

        public async Task<ActionResult> EditarNotificacion(int id, Notificacion notificacion)
        {
            var editarnotificacion = await _context.Notificacions.FindAsync(id);

            if (editarnotificacion == null)
            {
                return NotFound();
            }
            editarnotificacion.Titulo = notificacion.Titulo;
            editarnotificacion.Mensaje = notificacion.Mensaje;
            editarnotificacion.Tipo = notificacion.Tipo;
            editarnotificacion.FechaPublicacion = notificacion.FechaPublicacion;
            editarnotificacion.Estado = notificacion.Estado;

            await _context.SaveChangesAsync();

            return Ok(editarnotificacion);

        }
        [HttpDelete("EliminarNotificacion/{id}")]

        public async Task<ActionResult> EliminarNotificacion(int id)
        {
            var Notificacion = await _context.Notificacions.FindAsync(id);
            if (Notificacion == null)
            {
                return NotFound();
            }
            _context.Notificacions.Remove(Notificacion);
            await _context.SaveChangesAsync();
            return Ok(Notificacion);
        }

        



    }
}
