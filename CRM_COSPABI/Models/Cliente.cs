using System;
using System.Collections.Generic;

namespace CRM_COSPABI.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string CodigoFijo { get; set; } = null!;

    public string CodigoUbicacion { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string Ci { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public string Actividad { get; set; } = null!;

    public DateOnly FechaRegistro { get; set; }

    public bool Estado { get; set; }

    public int RolIdRol { get; set; }

    public virtual ICollection<CuentaCliente> CuentaClientes { get; set; } = new List<CuentaCliente>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Lectura> Lecturas { get; set; } = new List<Lectura>();

    public virtual ICollection<NotificacionCliente> NotificacionClientes { get; set; } = new List<NotificacionCliente>();

    public virtual Rol RolIdRolNavigation { get; set; } = null!;
}
