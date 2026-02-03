using CRM_COSPABI.DTOs;
using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Service
{
    public class FacturaService : IFacturaService
    {
        private readonly CospabicrmContext _context;

        public FacturaService(CospabicrmContext context)
        {
            _context = context;
        }

        public async Task<FacturaDto> GenerarFacturaAsync(GenerarFacturaDto dto)
        {
            // 1. Obtener Lectura
            var lectura = await _context.Lecturas
                .Include(l => l.ClienteIdClienteNavigation)
                .FirstOrDefaultAsync(l => l.IdLectura == dto.IdLectura);

            if (lectura == null)
                throw new Exception("Lectura no encontrada");

            // Validar si ya existe factura para esa lectura
            var existe = await _context.Facturas.AnyAsync(f => f.LecturaIdLectura == dto.IdLectura);
            if (existe)
                throw new Exception("Ya existe una factura para esta lectura");

            // 2. Calcular Totales (Lógica Provisional)
            // TODO: Mover esto a una configuración o tabla de tarifas
            decimal tarifaPorM3 = 5.0m; 
            decimal cargoFijo = 10.0m;

            var consumo = lectura.ConsumoM3;
            var costoConsumo = consumo * tarifaPorM3;
            var total = costoConsumo + cargoFijo;

            // 3. Crear Entidad Factura
            var factura = new Factura
            {
                Periodo = lectura.FechaLectura, // Asumimos periodo = fecha lectura por ahora
                FechaEmision = dto.FechaEmision,
                FechaVencimiento = dto.FechaVencimiento,
                TotalConsumo = costoConsumo,
                TotalFactura = total,
                Estado = "PENDIENTE",
                DeudaActual = total,
                ClienteIdCliente = lectura.ClienteIdCliente,
                LecturaIdLectura = lectura.IdLectura
            };

            // 4. Agregar Detalles
            factura.DetalleFacturas.Add(new DetalleFactura
            {
                Concepto = "Cargo Fijo",
                MontoUnitario = cargoFijo,
                Importe = 1, // Usando Importe como cantidad/unidad
                SubTotal = cargoFijo
            });

            factura.DetalleFacturas.Add(new DetalleFactura
            {
                Concepto = $"Consumo de Agua ({consumo} m3)",
                MontoUnitario = tarifaPorM3,
                Importe = consumo, // Usando Importe como cantidad
                SubTotal = costoConsumo
            });

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return MapToDto(factura, lectura.ClienteIdClienteNavigation);
        }

        public async Task<FacturaDto?> ObtenerPorIdAsync(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.ClienteIdClienteNavigation)
                .Include(f => f.DetalleFacturas)
                .FirstOrDefaultAsync(f => f.IdFactura == id);

            if (factura == null) return null;

            return MapToDto(factura, factura.ClienteIdClienteNavigation);
        }

        public async Task<IEnumerable<FacturaDto>> ListarAsync()
        {
            var facturas = await _context.Facturas
                .Include(f => f.ClienteIdClienteNavigation)
                .Include(f => f.DetalleFacturas) // Incluir detalles si se quieren ver en lista
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync();

            return facturas.Select(f => MapToDto(f, f.ClienteIdClienteNavigation));
        }

        public async Task<IEnumerable<FacturaDto>> ListarPorClienteAsync(int idCliente)
        {
            var facturas = await _context.Facturas
                .Include(f => f.ClienteIdClienteNavigation)
                .Include(f => f.DetalleFacturas)
                .Where(f => f.ClienteIdCliente == idCliente)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync();

            return facturas.Select(f => MapToDto(f, f.ClienteIdClienteNavigation));
        }

        public async Task<bool> AnularFacturaAsync(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null) return false;

            factura.Estado = "ANULADA";
            factura.DeudaActual = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        private static FacturaDto MapToDto(Factura f, Cliente c)
        {
            return new FacturaDto
            {
                IdFactura = f.IdFactura,
                Periodo = f.Periodo.ToString(),
                FechaEmision = f.FechaEmision,
                FechaVencimiento = f.FechaVencimiento,
                TotalConsumo = f.TotalConsumo,
                TotalFactura = f.TotalFactura,
                Estado = f.Estado,
                DeudaActual = f.DeudaActual,
                ClienteId = f.ClienteIdCliente,
                ClienteNombre = c.NombreCompleto,
                LecturaId = f.LecturaIdLectura,
                Detalles = f.DetalleFacturas.Select(d => new DetalleFacturaDto
                {
                    Concepto = d.Concepto,
                    Importe = d.Importe,
                    MontoUnitario = d.MontoUnitario,
                    SubTotal = d.SubTotal
                }).ToList()
            };
        }
    }
}
