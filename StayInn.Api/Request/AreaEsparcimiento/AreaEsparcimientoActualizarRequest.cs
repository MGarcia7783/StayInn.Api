using System.ComponentModel.DataAnnotations;

namespace StayInn.Api.Request.AreaEsparcimiento
{
    public class AreaEsparcimientoActualizarRequest
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del área es obligatorio.")]
        [MaxLength(50)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción del área es obligatoria.")]
        [MaxLength(500)]
        public string Descripcion { get; set; } = null!;


        public IFormFile? ImagenUrl { get; set; }
    }
}
