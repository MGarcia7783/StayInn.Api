using System.ComponentModel.DataAnnotations;

namespace StayInn.Api.Request.Hotel
{
    public class HotelCrearRequest
    {
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


        [Required(ErrorMessage = "La latitud del hotel es obligatoria.")]
        [Range(-90, 90, ErrorMessage = "La latitud del hotel debe estar entre -90 y 90 grados.")]
        public decimal Latitud { get; set; }


        [Required(ErrorMessage = "La longitud del hotel es obligatoria.")]
        [Range(-180, 180, ErrorMessage = "La longitud del hotel debe estar entre -180 y 180 grados.")]
        public decimal Longitud { get; set; }


        [Required(ErrorMessage = "La imagen principal del hotel es obligatoria.")]
        public IFormFile ImagenPrincipal { get; set; } = null!;
    }
}
