using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Domain.Enums;
using StayInn.Infrastructure.Persistence.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class HabitacionRepository : IHabitacionRepository
    {
        private readonly ApplicationDbContext _context;

        public HabitacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Habitacion habitacion)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Habitacion>> BuscarHabitacion(string valor, int pagina, int tamanoPagina)
        {
            var valorNormalizado = valor.Trim().ToLower();
            var pattern = $"%{valorNormalizado}%";

            return await _context.Habitaciones
                .Where(h =>
                    EF.Functions.Like(h.Numero.ToLower(), pattern) ||
                    EF.Functions.Like(h.Descripcion.ToLower(), pattern))
                .OrderBy(h => h.Numero)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }

        public async Task CambiarEstadoAsync(Habitacion habitacion)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> ContarBusquedaAsync(string valor)
        {
            var valorNormalizado = valor.Trim().ToLower();
            var pattern = $"%{valorNormalizado}%";

            return await _context.Habitaciones
                .Where(h =>
                    EF.Functions.Like(h.Numero.ToLower(), pattern) ||
                    EF.Functions.Like(h.Descripcion.ToLower(), pattern))
                .CountAsync();
        }

        public async Task<int> ContarDisponiblesAsync()
            => await _context.Habitaciones.CountAsync(h => h.EstaDisponible);

        public async Task<int> ContarTodasAsync()
            => await _context.Habitaciones.CountAsync();

        public async Task CrearAsync(Habitacion habitacion)
        {
            _context.Habitaciones.Add(habitacion);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
            => await _context.Habitaciones
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync();

        public Task<bool> ExisteNumeroHabitacionAsync(string numero)
        {
            var normalizarNumero = numero.Trim().ToLower();
            
            return _context.Habitaciones.AnyAsync(h => h.Numero.ToLower() == normalizarNumero);
        }

        public async Task<IEnumerable<Habitacion>> ObtenerDisponiblesAsync(int pagina, int tamanoPagina)
            => await _context.Habitaciones
                .Where(h => h.EstaDisponible)
                .OrderBy(h => h.Numero)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

        public async Task<Habitacion?> ObtenerPorIdAsync(int id)
            => await _context.Habitaciones.FindAsync(id);

        public async Task<IEnumerable<Habitacion>> ObtenerTodasAsync(int pagina, int tamanoPagina)
            => await _context.Habitaciones
                .OrderByDescending(h => h.EstaDisponible)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

        public async Task<bool> TieneReservacionActivaAsync(int habitacionId)
            => await _context.Reservaciones
                .AnyAsync (r =>
                    r.HabitacionId == habitacionId &&
                    (r.Estado == EstadoReservacion.Pendiente ||
                     r.Estado == EstadoReservacion.Confirmada));

        public async Task<bool> TienereservacionAsociada(int id)
            => await _context.Reservaciones
                .AnyAsync(r => r.HabitacionId == id);
    }
}
