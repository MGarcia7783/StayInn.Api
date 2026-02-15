using System.ComponentModel.DataAnnotations;

namespace StayInn.Application.DTOs.Usuario
{
    public class UsuarioRegistroDto
    {
        [Required]
        [MaxLength(70, ErrorMessage = "El nombre no debe exceder los 70 caracteres.")]
        public string NombreCompleto { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
