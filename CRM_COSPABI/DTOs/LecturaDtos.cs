using System.ComponentModel.DataAnnotations;

namespace CRM_COSPABI.DTOs
{
    public class LecturaCreateDto
    {
        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int LecturaActual { get; set; }

        [Required]
        public DateOnly FechaLectura { get; set; }

        public string Observacion { get; set; }
    }

    public class LecturaResponseDto
    {
        public int IdLectura { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateOnly FechaLectura { get; set; }
        public int LecturaAnterior { get; set; }
        public int LecturaActual { get; set; } // Esto está implícito por LecturaAnterior + Consumo usualmente, pero mejor retornar el valor calculado si se almacena o deriva. Espera, solo ConsumoM3 se almacena. Entonces Actual = Anterior + Consumo.
        public decimal ConsumoM3 { get; set; }
        public int DiasFacturados { get; set; }
        public string Observacion { get; set; }
    }
}
