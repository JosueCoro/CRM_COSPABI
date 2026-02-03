namespace CRM_COSPABI.DTOs
{
    public class GenerarFacturaDto
    {
        public int IdLectura { get; set; }
        public DateOnly FechaEmision { get; set; }
        public DateOnly FechaVencimiento { get; set; }
    }
}
