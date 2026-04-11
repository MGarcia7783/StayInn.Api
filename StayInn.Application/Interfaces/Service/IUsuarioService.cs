using StayInn.Application.DTOs.Habitacion;
using StayInn.Application.DTOs.Usuario;
using StayInn.Application.Response;

namespace StayInn.Application.Interfaces.Service
{
    public interface IUsuarioService
    {
        Task<LoginRespuestaUsuarioDto> LoginAsync(UsuarioLoginDto dto);


        Task<UsuarioDto> RegistrarUsuario(UsuarioRegistroDto dto);
        Task CambiarEstadoAsync(string id, bool activo);


        Task<ICollection<UsuarioDto>> ObtenerUsuariosAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<UsuarioDto>> BuscarUsuarioAsync(string valor, int pagina, int tamanoPagina);
        Task<UsuarioDto> ObtenerUsuarioPorIdAsync(string id);
        Task<int> ContarAsync();
        Task<int> ContarBusquedaAsync(string valor);
    }
}
