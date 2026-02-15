using System.ComponentModel.DataAnnotations;

namespace StayInn.Application.DTOs.Usuario
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El email es requerido.")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "La contraseña es requerido.")]
        public string Password { get; set; } = null!;
    }
}
