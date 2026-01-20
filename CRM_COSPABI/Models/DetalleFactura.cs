using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class DetalleFactura
{
    public int IdDetalle { get; set; }

    public string Concepto { get; set; } = null!;

    public decimal Importe { get; set; }

    public decimal MontoUnitario { get; set; }

    public decimal SubTotal { get; set; }

    public int FacturaIdFactura { get; set; }

    public virtual Factura FacturaIdFacturaNavigation { get; set; } = null!;
}
