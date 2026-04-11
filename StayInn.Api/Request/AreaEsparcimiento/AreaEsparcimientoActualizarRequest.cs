using System.ComponentModel.DataAnnotations;

namespace StayInn.Api.Request.AreaEsparcimiento
{
    public class AreaEsparcimientoActualizarRequest
    {

        [Required(ErrorMessage = "El nombre del área es obligatorio.")]
        [MaxLength(50)]
        public string Nombre { get; set; } = null!;

        public IFormFile? ImagenUrl { get; set; }
    }
}
