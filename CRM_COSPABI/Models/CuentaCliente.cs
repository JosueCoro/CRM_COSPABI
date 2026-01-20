using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class CuentaCliente
{
    public int IdCuenta { get; set; }

    public string Usuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public DateTime UltimoAcceso { get; set; }

    public bool Estado { get; set; }

    public int ClienteIdCliente { get; set; }

    public virtual Cliente ClienteIdClienteNavigation { get; set; } = null!;
}
