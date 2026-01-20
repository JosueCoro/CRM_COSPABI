using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class UsuarioAdmin
{
    public int IdUsuarioAdmin { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public bool Estado { get; set; }

    public DateOnly FechaCreacion { get; set; }

    public int RolIdRol { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Lectura> Lecturas { get; set; } = new List<Lectura>();

    public virtual Rol RolIdRolNavigation { get; set; } = null!;
}
