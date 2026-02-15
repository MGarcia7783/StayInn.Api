using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IReservacionRepository
    {
        Task<Reservacion?> GetByIdAsync(int id);
        Task<IEnumerable<Reservacion>> GetByUsuarioAsync(string usuarioId);
        

        Task CreateAsync(Reservacion reservacion);
        Task UpdateAsync(Reservacion reservacion);
    }
}
