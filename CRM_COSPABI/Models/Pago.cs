using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public DateOnly FechaPago { get; set; }

    public string MetodoPago { get; set; } = null!;

    public decimal MontoPagado { get; set; }

    public int NumeroRecibo { get; set; }

    public string Cajero { get; set; } = null!;

    public bool Estado { get; set; }

    public int FacturaIdFactura { get; set; }

    public virtual ICollection<ComprobantePago> ComprobantePagos { get; set; } = new List<ComprobantePago>();

    public virtual Factura FacturaIdFacturaNavigation { get; set; } = null!;
}
