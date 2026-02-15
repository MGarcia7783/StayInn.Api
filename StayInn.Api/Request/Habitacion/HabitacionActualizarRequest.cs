using System.ComponentModel.DataAnnotations;

namespace StayInn.Api.Request.Habitacion
{
    public class HabitacionActualizarRequest
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de habitación es requerido.")]
        [StringLength(10, ErrorMessage = "El número de habitación no puede exceder los 10 caracteres.")]
        public string Numero { get; set; } = null!;


        [Required(ErrorMessage = "La capacidad de la habitación es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad de la habitación debe ser al menos 1.")]
        public int CapacidadMax { get; set; }


        [Required(ErrorMessage = "La descripción de la habitación es requerida.")]
        [StringLength(500, ErrorMessage = "La descripción de la habitación no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = null!;


        [Required(ErrorMessage = "El precio por noche de la habitación es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio por noche de la habitación debe ser un valor positivo.")]
        public decimal PrecioNoche { get; set; }
        public IFormFile? ImagenUrl { get; set; }
    }
}
