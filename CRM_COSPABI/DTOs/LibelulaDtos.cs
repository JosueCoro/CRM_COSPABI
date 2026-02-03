namespace CRM_COSPABI.DTOs
{
    // Solicitud a Libélula (nombres de propiedades JSON coinciden con requisitos API)
    public class LibelulaDeudaRequestDto
    {
        public string deuda_id { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public string moneda { get; set; } = "BOB";
        public LibelulaClienteDto cliente { get; set; }
        public string callback_url { get; set; }
        public string return_url { get; set; }
    }

    public class LibelulaClienteDto
    {
        public string nombre { get; set; }
        public string ci { get; set; }
        public string codigo { get; set; }
        public string correo { get; set; } 
        public string telefono { get; set; }
    }

    // Respuesta de Libélula
    public class LibelulaDeudaResponseDto
    {
        public string url_pasarela { get; set; }
        public string id_deuda { get; set; }
        public string imagen_qr { get; set; }
    }

    // Carga útil del Callback
    public class LibelulaCallbackDto
    {
        public string transaction_id { get; set; } // Campo común de Libélula
        public string deuda_id { get; set; } // Coincide con la solicitud
        public int estado { get; set; } 
        public decimal? monto { get; set; }
        public string fecha { get; set; }
        public string metodo_pago { get; set; }
        public string id_transaccion { get; set; } // Mantener el interno por si acaso o mapeado
    }
}
