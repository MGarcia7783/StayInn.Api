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

        public async Task<bool> ExisteReservacionActivaEnRangoAsync(int habitacionId, DateOnly fechaEntrada, DateOnly fechaSalida, int? reservacionId = null)
        {
            return await _context.Reservaciones
                .Where(r =>
                    (reservacionId == null || r.Id != reservacionId) &&
                    r.HabitacionId == habitacionId &&
                    (r.Estado == EstadoReservacion.Pendiente ||
                     r.Estado == EstadoReservacion.Confirmada) &&
                    fechaEntrada < r.FechaSalida &&
                    fechaSalida > r.FechaEntrada
                )
                .AnyAsync();
        }

        public async Task<int> GuardarCambiosAsync()
            => await _context.SaveChangesAsync();

        public async Task<Reservacion?> ObtenerPorIdAsync(int id)
            => await _context.Reservaciones
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Reservacion>> ObtenerPorUsuarioAsync(string usuarioId)
            => await _context.Reservaciones
                .Include(r =>r.Habitacion)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

        public async Task<IEnumerable<Reservacion>> ObtenerTodasAsync(int pagina, int tamanoPagina)
        => await _context.Reservaciones
            .Include(r => r.Habitacion)
            .OrderByDescending(r => r.FechaEntrada)
            .ThenByDescending(r => r.Estado == EstadoReservacion.Pendiente)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();
    }
}
