using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class NotificacionCliente
{
    public int IdNotificacionCliente { get; set; }

    public DateOnly FechaLectura { get; set; }

    public bool Leido { get; set; }

    public int NotificacionIdNotificacion { get; set; }

    public int ClienteIdCliente { get; set; }

    public virtual Cliente ClienteIdClienteNavigation { get; set; } = null!;

    public virtual Notificacion NotificacionIdNotificacionNavigation { get; set; } = null!;
}
