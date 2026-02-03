
namespace CRM_COSPABI.DTOs
{
    public class NotificacionDto
    {
        public int IdNotificacion { get; set; }
        public string Titulo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public DateOnly FechaPublicacion { get; set; }
        public bool Estado { get; set; }
    }

    public class CreateNotificacionDto
    {
        public string Titulo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public string Tipo { get; set; } = null!;
    }

    public class UpdateNotificacionDto
    {
        public string Titulo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public bool Estado { get; set; }
    }
}
