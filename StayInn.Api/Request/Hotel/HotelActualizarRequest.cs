using System.ComponentModel.DataAnnotations;

namespace StayInn.Api.Request.Hotel
{
    public class HotelActualizarRequest
    {
        [Required]
        public int Id { get; set; }


        [Required(ErrorMessage = "El nombre del hotel es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre del hotel no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = null!;


        [Required(ErrorMessage = "El email del hotel es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email del hotel no es válido.")]
        [MaxLength(100, ErrorMessage = "El email del hotel no puede superar los 100 caracteres.")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "El teléfono del hotel es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El teléfono del hotel no puede superar los 20 caracteres.")]
        public string Telefono { get; set; } = null!;


        [Required(ErrorMessage = "La descripción del hotel es obligatoria.")]
        [MaxLength(500, ErrorMessage = "La descripción del hotel no puede superar los 500 caracteres.")]
        public string Descripcion { get; set; } = null!;


        [Required(ErrorMessage = "La dirección del hotel es obligatoria.")]
        [MaxLength(250, ErrorMessage = "La dirección del hotel no puede superar los 250 caracteres.")]
        public string Direccion { get; set; } = null!;


        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        public IFormFile? ImagenPrincipal { get; set; }
    }
}
