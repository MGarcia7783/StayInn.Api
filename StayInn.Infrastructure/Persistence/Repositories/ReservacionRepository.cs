using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Domain.Enums;
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

        public async Task ActualizarAsync(Reservacion reservacion)
        {
            _context.Reservaciones.Update(reservacion);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ContarTodasAsync()
            => await _context.Reservaciones.CountAsync();

        public async Task CrearAsync(Reservacion reservacion)
        {
            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();
        }

        public async Task<Reservacion?> ObtenerPorIdAsync(int id)
            => await _context.Reservaciones.FindAsync(id);

        public async Task<IEnumerable<Reservacion>> ObtenerPorUsuarioAsync(string usuarioId)
            => await _context.Reservaciones
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

        public async Task<IEnumerable<Reservacion>> ObtenerTodasAsync(int pagina, int tamanoPagina)
        => await _context.Reservaciones
            .OrderByDescending(r => r.FechaEntrada)
            .ThenByDescending(r => r.Estado == EstadoReservacion.Pendiente)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();
    }
}
