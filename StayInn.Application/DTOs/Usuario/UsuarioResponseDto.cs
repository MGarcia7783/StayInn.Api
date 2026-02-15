
namespace StayInn.Application.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public string Id { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
