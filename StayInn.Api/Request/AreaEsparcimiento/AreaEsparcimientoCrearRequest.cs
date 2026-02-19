using System.ComponentModel.DataAnnotations;

namespace StayInn.Api.Request.AreaEsparcimiento
{
    public class AreaEsparcimientoCrearRequest
    {
        [Required(ErrorMessage = "El nombre del área es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción del área es obligatoria.")]
        [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La imagen del área es obligatoria.")]
        public IFormFile ImagenUrl { get; set; } = null!;
    }
}
