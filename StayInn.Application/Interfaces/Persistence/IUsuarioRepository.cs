using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IUsuarioRepository
    {
        Task<ApplicationUser?> ObtenerPorIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> ObtenerTodosAsync(int pagina, int tamanoPagina);
        Task<int> ContarAsync();
    }
}
