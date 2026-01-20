using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public DateOnly Periodo { get; set; }

    public DateOnly FechaEmision { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public decimal TotalConsumo { get; set; }

    public decimal TotalFactura { get; set; }

    public string Estado { get; set; } = null!;

    public decimal DeudaActual { get; set; }

    public int ClienteIdCliente { get; set; }

    public int LecturaIdLectura { get; set; }

    public virtual Cliente ClienteIdClienteNavigation { get; set; } = null!;

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual Lectura LecturaIdLecturaNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
