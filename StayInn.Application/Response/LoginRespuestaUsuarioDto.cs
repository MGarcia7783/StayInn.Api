using StayInn.Application.DTOs.Usuario;

namespace StayInn.Application.Response
{
    public class LoginRespuestaUsuarioDto
    {
        public UsuarioDto? Usuario { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
