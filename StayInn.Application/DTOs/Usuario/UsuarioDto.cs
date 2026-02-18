
namespace StayInn.Application.DTOs.Usuario
{
    public class UsuarioDto
    {
        public string Id { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
