using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class ComprobantePago
{
    public int IdComprobante { get; set; }

    public string Tipo { get; set; } = null!;

    public string Referencia { get; set; } = null!;

    public string EntidadPago { get; set; } = null!;

    public DateOnly? FechaConfirmacion { get; set; }

    public int PagoIdPago { get; set; }

    public virtual Pago PagoIdPagoNavigation { get; set; } = null!;
}
