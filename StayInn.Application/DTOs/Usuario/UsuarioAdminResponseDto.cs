
namespace StayInn.Application.DTOs.Usuario
{
    public class UsuarioAdminResponseDto
    {
        public string Id { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
