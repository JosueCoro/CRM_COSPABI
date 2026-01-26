using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class RolPermiso
{
    public int IdRolPermiso { get; set; }

    public int PermisoIdPermiso { get; set; }

    public int RolIdRol { get; set; }

    public virtual Permiso PermisoIdPermisoNavigation { get; set; } = null!;

    public virtual Rol RolIdRolNavigation { get; set; } = null!;
}
