using CRM_COSPABI.DTOs;
using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRM_COSPABI.Service
{
    public class LecturaService : ILecturaService
    {
        private readonly CospabicrmContext _context;

        public LecturaService(CospabicrmContext context)
        {
            _context = context;
        }

        public async Task<LecturaResponseDto> RegistrarLecturaAsync(LecturaCreateDto lecturaDto, int idUsuarioAdmin)
        {
            // 1. Obtener última lectura del cliente
            var ultimaLectura = await _context.Lecturas
                .Where(l => l.ClienteIdCliente == lecturaDto.IdCliente)
                .OrderByDescending(l => l.FechaLectura)
                .FirstOrDefaultAsync();

            int lecturaAnterior = 0;
            DateTime fechaAnterior = DateTime.Parse("2020-01-01"); // Valor por defecto

            if (ultimaLectura != null)
            {
                lecturaAnterior = ultimaLectura.LecturaAnterior + (int)ultimaLectura.ConsumoM3;
                fechaAnterior = ultimaLectura.FechaLectura.ToDateTime(TimeOnly.MinValue);
            }

            if (lecturaDto.LecturaActual < lecturaAnterior)
            {
                throw new Exception($"La lectura actual ({lecturaDto.LecturaActual}) no puede ser menor a la anterior ({lecturaAnterior}).");
            }

            decimal consumo = lecturaDto.LecturaActual - lecturaAnterior;
            
            // Cálculo de días (aproximado)
            int dias = (lecturaDto.FechaLectura.ToDateTime(TimeOnly.MinValue) - fechaAnterior).Days;
            if (dias < 0) dias = 0; 
            
            // Si es la primera lectura, los días pueden ser muchos o irrelevantes. 
            // En un escenario real, podríamos querer validar contra "FechaRegistro" del cliente si no existe lectura previa.
            if (ultimaLectura == null)
            {
                 var clienteReg = await _context.Clientes.FindAsync(lecturaDto.IdCliente);
                 if (clienteReg != null) 
                 {
                    dias = (lecturaDto.FechaLectura.ToDateTime(TimeOnly.MinValue) - clienteReg.FechaRegistro.ToDateTime(TimeOnly.MinValue)).Days;
                 }
            }

            var nuevaLectura = new Lectura
            {
                ClienteIdCliente = lecturaDto.IdCliente,
                FechaLectura = lecturaDto.FechaLectura,
                LecturaAnterior = lecturaAnterior,
                ConsumoM3 = consumo,
                DiasFacturados = dias,
                Observacion = lecturaDto.Observacion ?? "Lectura regular",
                UsuarioAdminIdUsuarioAdmin = idUsuarioAdmin
            };

            _context.Lecturas.Add(nuevaLectura);
            await _context.SaveChangesAsync();
            
            // --- INICIO GENERACIÓN AUTOMÁTICA DE FACTURA ---
            // Tarifa: 1.66 Bs por m3
            decimal tarifaPorM3 = 1.66m;
            decimal tarifaBase = 15.00m; // Tarifa fija por servicio

            decimal totalConsumo = consumo * tarifaPorM3;
            
            // Total Factura = Consumo variable + Tarifa Base
            // No hay alcantarillado.
            decimal totalFactura = totalConsumo + tarifaBase;

            var nuevaFactura = new Factura
            {
                ClienteIdCliente = lecturaDto.IdCliente,
                LecturaIdLectura = nuevaLectura.IdLectura,
                Periodo = DateOnly.FromDateTime(new DateTime(lecturaDto.FechaLectura.Year, lecturaDto.FechaLectura.Month, 1)), // Primer día del mes de lectura
                FechaEmision = DateOnly.FromDateTime(DateTime.Now),
                FechaVencimiento = DateOnly.FromDateTime(DateTime.Now.AddDays(10)), // Vence en 10 días por defecto
                TotalConsumo = totalConsumo,
                DeudaActual = totalFactura,
                TotalFactura = totalFactura,
                Estado = "PENDIENTE"
            };

            _context.Facturas.Add(nuevaFactura);
            await _context.SaveChangesAsync();

            // Detalle 1: Consumo de Agua
            var detalleConsumo = new DetalleFactura
            {
                FacturaIdFactura = nuevaFactura.IdFactura,
                Concepto = $"Consumo de Agua Potable ({consumo} m3)",
                MontoUnitario = tarifaPorM3,
                Importe = consumo, 
                SubTotal = totalConsumo
            };
            _context.DetalleFacturas.Add(detalleConsumo);

            // Detalle 2: Tarifa Base
            var detalleBase = new DetalleFactura
            {
                FacturaIdFactura = nuevaFactura.IdFactura,
                Concepto = "Tarifa Básica (Mantenimiento y Servicio)",
                MontoUnitario = tarifaBase,
                Importe = 1, // Unidad
                SubTotal = tarifaBase
            };
            _context.DetalleFacturas.Add(detalleBase);

            await _context.SaveChangesAsync();
            // --- FIN GENERACIÓN AUTOMÁTICA DE FACTURA ---
            
            var cliente = await _context.Clientes.FindAsync(lecturaDto.IdCliente);

            return new LecturaResponseDto
            {
                IdLectura = nuevaLectura.IdLectura,
                IdCliente = nuevaLectura.ClienteIdCliente,
                NombreCliente = cliente?.NombreCompleto ?? "Desconocido",
                FechaLectura = nuevaLectura.FechaLectura,
                LecturaAnterior = nuevaLectura.LecturaAnterior,
                LecturaActual = lecturaAnterior + (int)consumo,
                ConsumoM3 = consumo,
                DiasFacturados = dias,
                Observacion = nuevaLectura.Observacion
            };
        }

        public async Task<List<LecturaResponseDto>> ObtenerLecturasPorClienteAsync(int idCliente)
        {
            var lecturas = await _context.Lecturas
                .Include(l => l.ClienteIdClienteNavigation)
                .Where(l => l.ClienteIdCliente == idCliente)
                .OrderByDescending(l => l.FechaLectura)
                .ToListAsync();

            return lecturas.Select(l => new LecturaResponseDto
            {
                IdLectura = l.IdLectura,
                IdCliente = l.ClienteIdCliente,
                NombreCliente = l.ClienteIdClienteNavigation.NombreCompleto,
                FechaLectura = l.FechaLectura,
                LecturaAnterior = l.LecturaAnterior,
                LecturaActual = l.LecturaAnterior + (int)l.ConsumoM3,
                ConsumoM3 = l.ConsumoM3,
                DiasFacturados = l.DiasFacturados,
                Observacion = l.Observacion
            }).ToList();
        }

        public async Task<LecturaResponseDto> ObtenerLecturaAsync(int idLectura)
        {
             var l = await _context.Lecturas
                .Include(l => l.ClienteIdClienteNavigation)
                .FirstOrDefaultAsync(x => x.IdLectura == idLectura);

            if (l == null) return null;

            return new LecturaResponseDto
            {
                IdLectura = l.IdLectura,
                IdCliente = l.ClienteIdCliente,
                NombreCliente = l.ClienteIdClienteNavigation.NombreCompleto,
                FechaLectura = l.FechaLectura,
                LecturaAnterior = l.LecturaAnterior,
                LecturaActual = l.LecturaAnterior + (int)l.ConsumoM3,
                ConsumoM3 = l.ConsumoM3,
                DiasFacturados = l.DiasFacturados,
                Observacion = l.Observacion
            };
        }
    }
}
