using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Models;

public partial class CospabicrmContext : DbContext
{
    public CospabicrmContext()
    {
    }

    public CospabicrmContext(DbContextOptions<CospabicrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ComprobantePago> ComprobantePagos { get; set; }

    public virtual DbSet<CuentaCliente> CuentaClientes { get; set; }

    public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Lectura> Lecturas { get; set; }

    public virtual DbSet<Notificacion> Notificacions { get; set; }

    public virtual DbSet<NotificacionCliente> NotificacionClientes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolPermiso> RolPermisos { get; set; }

    public virtual DbSet<UsuarioAdmin> UsuarioAdmins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HS9CBF1\\SQLEXPRESS;Database=COSPABICRM;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("cliente_PK");

            entity.ToTable("cliente");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Actividad)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("actividad");
            entity.Property(e => e.Categoria)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Ci)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ci");
            entity.Property(e => e.CodigoFijo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigo_fijo");
            entity.Property(e => e.CodigoUbicacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigo_ubicacion");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.RolIdRol).HasColumnName("rol_id_rol");

            entity.HasOne(d => d.RolIdRolNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.RolIdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_rol_FK");
        });

        modelBuilder.Entity<ComprobantePago>(entity =>
        {
            entity.HasKey(e => e.IdComprobante).HasName("comprobante_pago_PK");

            entity.ToTable("comprobante_pago");

            entity.Property(e => e.IdComprobante).HasColumnName("id_comprobante");
            entity.Property(e => e.EntidadPago)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("entidad_pago");
            entity.Property(e => e.FechaConfirmacion).HasColumnName("fecha_confirmacion");
            entity.Property(e => e.PagoIdPago).HasColumnName("pago_id_pago");
            entity.Property(e => e.Referencia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("referencia");
            entity.Property(e => e.Tipo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.PagoIdPagoNavigation).WithMany(p => p.ComprobantePagos)
                .HasForeignKey(d => d.PagoIdPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comprobante_pago_pago_FK");
        });

        modelBuilder.Entity<CuentaCliente>(entity =>
        {
            entity.HasKey(e => e.IdCuenta).HasName("cuenta_cliente_PK");

            entity.ToTable("cuenta_cliente");

            entity.Property(e => e.IdCuenta).HasColumnName("id_cuenta");
            entity.Property(e => e.ClienteIdCliente).HasColumnName("cliente_id_cliente");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.UltimoAcceso)
                .HasColumnType("datetime")
                .HasColumnName("ultimo_acceso");
            entity.Property(e => e.Usuario)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("usuario");

            entity.HasOne(d => d.ClienteIdClienteNavigation).WithMany(p => p.CuentaClientes)
                .HasForeignKey(d => d.ClienteIdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cuenta_cliente_cliente_FK");
        });

        modelBuilder.Entity<DetalleFactura>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("detalle_factura_PK");

            entity.ToTable("detalle_factura");

            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.Concepto)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("concepto");
            entity.Property(e => e.FacturaIdFactura).HasColumnName("factura_id_factura");
            entity.Property(e => e.Importe)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("importe");
            entity.Property(e => e.MontoUnitario)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("monto_unitario");
            entity.Property(e => e.SubTotal)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("sub_total");

            entity.HasOne(d => d.FacturaIdFacturaNavigation).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.FacturaIdFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalle_factura_factura_FK");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("factura_PK");

            entity.ToTable("factura");

            entity.Property(e => e.IdFactura).HasColumnName("id_factura");
            entity.Property(e => e.ClienteIdCliente).HasColumnName("cliente_id_cliente");
            entity.Property(e => e.DeudaActual)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("deuda_actual");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.LecturaIdLectura).HasColumnName("lectura_id_lectura");
            entity.Property(e => e.Periodo).HasColumnName("periodo");
            entity.Property(e => e.TotalConsumo)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("total_consumo");
            entity.Property(e => e.TotalFactura)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("total_factura");

            entity.HasOne(d => d.ClienteIdClienteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.ClienteIdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factura_cliente_FK");

            entity.HasOne(d => d.LecturaIdLecturaNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.LecturaIdLectura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factura_lectura_FK");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("gasto_PK");

            entity.ToTable("gasto");

            entity.Property(e => e.IdGasto).HasColumnName("id_gasto");
            entity.Property(e => e.Concepto)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("concepto");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.UsuarioAdminIdUsuarioAdmin).HasColumnName("usuario_admin_id_usuario_admin");

            entity.HasOne(d => d.UsuarioAdminIdUsuarioAdminNavigation).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.UsuarioAdminIdUsuarioAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("gasto_usuario_admin_FK");
        });

        modelBuilder.Entity<Lectura>(entity =>
        {
            entity.HasKey(e => e.IdLectura).HasName("lectura_PK");

            entity.ToTable("lectura");

            entity.Property(e => e.IdLectura).HasColumnName("id_lectura");
            entity.Property(e => e.ClienteIdCliente).HasColumnName("cliente_id_cliente");
            entity.Property(e => e.ConsumoM3)
                .HasColumnType("numeric(30, 2)")
                .HasColumnName("consumo_m3");
            entity.Property(e => e.DiasFacturados).HasColumnName("dias_facturados");
            entity.Property(e => e.FechaLectura).HasColumnName("fecha_lectura");
            entity.Property(e => e.LecturaAnterior).HasColumnName("lectura_anterior");
            entity.Property(e => e.Observacion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observacion");
            entity.Property(e => e.UsuarioAdminIdUsuarioAdmin).HasColumnName("usuario_admin_id_usuario_admin");

            entity.HasOne(d => d.ClienteIdClienteNavigation).WithMany(p => p.Lecturas)
                .HasForeignKey(d => d.ClienteIdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lectura_cliente_FK");

            entity.HasOne(d => d.UsuarioAdminIdUsuarioAdminNavigation).WithMany(p => p.Lecturas)
                .HasForeignKey(d => d.UsuarioAdminIdUsuarioAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lectura_usuario_admin_FK");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("notificacion_PK");

            entity.ToTable("notificacion");

            entity.Property(e => e.IdNotificacion).HasColumnName("id_notificacion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaPublicacion).HasColumnName("fecha_publicacion");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("mensaje");
            entity.Property(e => e.Tipo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tipo");
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<NotificacionCliente>(entity =>
        {
            entity.HasKey(e => e.IdNotificacionCliente).HasName("notificacion_cliente_PK");

            entity.ToTable("notificacion_cliente");

            entity.Property(e => e.IdNotificacionCliente).HasColumnName("id_notificacion_cliente");
            entity.Property(e => e.ClienteIdCliente).HasColumnName("cliente_id_cliente");
            entity.Property(e => e.FechaLectura).HasColumnName("fecha_lectura");
            entity.Property(e => e.Leido).HasColumnName("leido");
            entity.Property(e => e.NotificacionIdNotificacion).HasColumnName("notificacion_id_notificacion");

            entity.HasOne(d => d.ClienteIdClienteNavigation).WithMany(p => p.NotificacionClientes)
                .HasForeignKey(d => d.ClienteIdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notificacion_cliente_cliente_FK");

            entity.HasOne(d => d.NotificacionIdNotificacionNavigation).WithMany(p => p.NotificacionClientes)
                .HasForeignKey(d => d.NotificacionIdNotificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notificacion_cliente_notificacion_FK");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("pago_PK");

            entity.ToTable("pago");

            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.Cajero)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("cajero");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FacturaIdFactura).HasColumnName("factura_id_factura");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.MontoPagado)
                .HasColumnType("decimal(30, 2)")
                .HasColumnName("monto_pagado");
            entity.Property(e => e.NumeroRecibo).HasColumnName("numero_recibo");

            entity.HasOne(d => d.FacturaIdFacturaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.FacturaIdFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pago_factura_FK");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("permiso_PK");

            entity.ToTable("permiso");

            entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");
            entity.Property(e => e.Codigo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("codigo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("rol_PK");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => e.IdRolPermiso).HasName("rol_permiso_PK");

            entity.ToTable("rol_permiso");

            entity.Property(e => e.IdRolPermiso).HasColumnName("id_rol_permiso");
            entity.Property(e => e.PermisoIdPermiso).HasColumnName("permiso_id_permiso");
            entity.Property(e => e.RolIdRol).HasColumnName("rol_id_rol");

            entity.HasOne(d => d.PermisoIdPermisoNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.PermisoIdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rol_permiso_permiso_FK");

            entity.HasOne(d => d.RolIdRolNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.RolIdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rol_permiso_rol_FK");
        });

        modelBuilder.Entity<UsuarioAdmin>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioAdmin).HasName("usuario_admin_PK");

            entity.ToTable("usuario_admin");

            entity.Property(e => e.IdUsuarioAdmin).HasColumnName("id_usuario_admin");
            entity.Property(e => e.Apellido)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Cargo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("cargo");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(550)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.RolIdRol).HasColumnName("rol_id_rol");
            entity.Property(e => e.Usuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("usuario");

            entity.HasOne(d => d.RolIdRolNavigation).WithMany(p => p.UsuarioAdmins)
                .HasForeignKey(d => d.RolIdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_admin_rol_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
