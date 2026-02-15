using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Infrastructure.Persistence.Data;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class ReservacionRepository : IReservacionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Reservacion reservacion)
        {
            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();
        }

        public async Task<Reservacion?> GetByIdAsync(int id)
            => await _context.Reservaciones.FindAsync(id);

        public async Task<IEnumerable<Reservacion>> GetByUsuarioAsync(string usuarioId)
            => await _context.Reservaciones
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

        public async Task UpdateAsync(Reservacion reservacion)
        {
            _context.Reservaciones.Update(reservacion);
            await _context.SaveChangesAsync();
        }
    }
}
