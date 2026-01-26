using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

    public virtual ICollection<UsuarioAdmin> UsuarioAdmins { get; set; } = new List<UsuarioAdmin>();
}
