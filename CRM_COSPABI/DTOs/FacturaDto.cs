using CRM_COSPABI.Models;

namespace CRM_COSPABI.DTOs
{
    public class FacturaDto
    {
        public int IdFactura { get; set; }
        public string Periodo { get; set; } = null!;
        public DateOnly FechaEmision { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public decimal TotalConsumo { get; set; }
        public decimal TotalFactura { get; set; }
        public string Estado { get; set; } = null!;
        public decimal DeudaActual { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = null!;
        public int LecturaId { get; set; }
        
        public List<DetalleFacturaDto> Detalles { get; set; } = new();
    }

    public class DetalleFacturaDto
    {
        public string Concepto { get; set; } = null!;
        public decimal Importe { get; set; }
        public decimal MontoUnitario { get; set; }
        public decimal SubTotal { get; set; }
    }
}
