using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Notificacion
{
    public int IdNotificacion { get; set; }

    public string Titulo { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public DateOnly FechaPublicacion { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<NotificacionCliente> NotificacionClientes { get; set; } = new List<NotificacionCliente>();
}
