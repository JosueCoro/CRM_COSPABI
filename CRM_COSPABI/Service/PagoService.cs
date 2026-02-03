using CRM_COSPABI.DTOs;
using CRM_COSPABI.Models;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace CRM_COSPABI.Service
{
    public class PagoService : IPagoService
    {
        private readonly CospabicrmContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PagoService(CospabicrmContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<LibelulaDeudaResponseDto> IniciarPagoAsync(IniciarPagoDto pagoDto)
        {
            // 1. Validar Factura
            var factura = await _context.Facturas
                .Include(f => f.ClienteIdClienteNavigation)
                .FirstOrDefaultAsync(f => f.IdFactura == pagoDto.IdFactura);

            if (factura == null)
                throw new Exception("Factura no encontrada.");

            if (factura.Estado == "PAGADA")
                throw new Exception("La factura ya está pagada.");

            var cliente = factura.ClienteIdClienteNavigation;

            // 2. Construir Request para Libélula
            var deudaId = $"FACTURA_{factura.IdFactura}";
            var request = new LibelulaDeudaRequestDto
            {
                deuda_id = deudaId,
                descripcion = $"Pago factura agua potable - Periodo {factura.Periodo:yyyy-MM}",
                monto = factura.DeudaActual,
                moneda = "BOB",
                cliente = new LibelulaClienteDto
                {
                    nombre = cliente.NombreCompleto,
                    ci = cliente.Ci,
                    codigo = cliente.CodigoFijo,
                    telefono = "00000000", // Predeterminado o buscar si está disponible
                    correo = "cliente@cospabi.com" // Predeterminado o buscar
                },
                callback_url = _configuration["Libelula:CallbackUrl"],
                return_url = _configuration["Libelula:ReturnUrl"]
            };

            // 3. Llamar a Libélula
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            
            // Cabecera de Autorización
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configuration["Libelula:AppKey"]);

            var response = await _httpClient.PostAsync($"{_configuration["Libelula:BaseUrl"]}/deuda/registrar", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error en pasarela de pagos: {response.StatusCode} - {errorBody}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultado = JsonSerializer.Deserialize<LibelulaDeudaResponseDto>(responseBody, options);

            return resultado;
        }

        public async Task<bool> ProcesarCallbackAsync(LibelulaCallbackDto callbackDto)
        {
            // 1. Parsear ID Factura
            if (!callbackDto.deuda_id.StartsWith("FACTURA_"))
                throw new Exception("Formato de ID de deuda inválido.");

            if (!int.TryParse(callbackDto.deuda_id.Replace("FACTURA_", ""), out int facturaId))
                throw new Exception("ID de factura inválido.");

            // 2. Obtener Factura
            var factura = await _context.Facturas.FindAsync(facturaId);
            if (factura == null) throw new Exception("Factura no encontrada.");
            
            if (factura.Estado == "PAGADA" || factura.DeudaActual == 0)
                return true;

            // 3. Registrar Pago
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                factura.Estado = "PAGADA";
                factura.DeudaActual = 0;
                
                _context.Facturas.Update(factura);


                var nuevoPago = new Pago
                {
                    FacturaIdFactura = factura.IdFactura,
                    FechaPago = DateOnly.FromDateTime(DateTime.Now),
                    MetodoPago = callbackDto.metodo_pago ?? "ONLINE",
                    MontoPagado = callbackDto.monto ?? factura.TotalFactura,
                    NumeroRecibo = (await _context.Pagos.MaxAsync(p => (int?)p.NumeroRecibo) ?? 0) + 1,
                    Cajero = "ONLINE_LIBELULA",
                    Estado = true // Pagado/Activo
                };

                _context.Pagos.Add(nuevoPago);
                await _context.SaveChangesAsync();

                var comprobante = new ComprobantePago
                {
                    PagoIdPago = nuevoPago.IdPago,
                    Tipo = "DIGITAL",
                    Referencia = callbackDto.transaction_id ?? callbackDto.id_transaccion ?? Guid.NewGuid().ToString(),
                    EntidadPago = "LIBELULA",
                    FechaConfirmacion = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.ComprobantePagos.Add(comprobante);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
