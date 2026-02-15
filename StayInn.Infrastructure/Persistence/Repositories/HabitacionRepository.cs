using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Infrastructure.Persistence.Data;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class HabitacionRepository : IHabitacionRepository
    {
        private readonly ApplicationDbContext _context;

        public HabitacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountDisponiblesAsync()
            => await _context.Habitaciones.CountAsync(h => h.EstaDisponible);

        public async Task CreateAsync(Habitacion habitacion)
        {
            _context.Habitaciones.Add(habitacion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
            => await _context.Habitaciones.Where(h => h.Id == id).ExecuteDeleteAsync();

        public async Task<Habitacion?> GetByIdAsync(int id)
            => await _context.Habitaciones.FindAsync(id);

        public async Task<IEnumerable<Habitacion>> GetDisponiblesAsync(int page, int pageSize)
            => await _context.Habitaciones
                .Where(h => h.EstaDisponible)
                .OrderBy(h => h.PrecioNoche)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task UpdateAsync(Habitacion habitacion)
        {
            _context.Habitaciones.Update(habitacion);
            await _context.SaveChangesAsync();
        }
    }
}
