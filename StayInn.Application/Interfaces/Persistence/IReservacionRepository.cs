using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IReservacionRepository
    {
        Task<Reservacion?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Reservacion>> ObtenerTodasAsync(int pagina, int tamanoPagina);
        Task<int> ContarTodasAsync(); 
        Task<IEnumerable<Reservacion>> ObtenerPorUsuarioAsync(string usuarioId);
        

        Task CrearAsync(Reservacion reservacion);
    }
}