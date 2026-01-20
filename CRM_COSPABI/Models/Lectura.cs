using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Lectura
{
    public int IdLectura { get; set; }

    public DateOnly FechaLectura { get; set; }

    public int LecturaAnterior { get; set; }

    public decimal ConsumoM3 { get; set; }

    public int DiasFacturados { get; set; }

    public string Observacion { get; set; } = null!;

    public int ClienteIdCliente { get; set; }

    public int UsuarioAdminIdUsuarioAdmin { get; set; }

    public virtual Cliente ClienteIdClienteNavigation { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual UsuarioAdmin UsuarioAdminIdUsuarioAdminNavigation { get; set; } = null!;
}
