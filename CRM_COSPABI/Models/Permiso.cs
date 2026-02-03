using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public string? Codigo { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}
