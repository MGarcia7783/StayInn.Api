using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IHabitacionRepository
    {
        Task<Habitacion?> GetByIdAsync(int id);
        Task<IEnumerable<Habitacion>> GetDisponiblesAsync(int page, int pageSize);
        Task<int> CountDisponiblesAsync();


        Task CreateAsync (Habitacion habitacion);
        Task UpdateAsync (Habitacion habitacion);
        Task DeleteAsync (int id);
    }
}
