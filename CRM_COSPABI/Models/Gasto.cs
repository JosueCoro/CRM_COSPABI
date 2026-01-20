using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Gasto
{
    public int IdGasto { get; set; }

    public string Concepto { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateOnly Fecha { get; set; }

    public int UsuarioAdminIdUsuarioAdmin { get; set; }

    public virtual UsuarioAdmin UsuarioAdminIdUsuarioAdminNavigation { get; set; } = null!;
}
