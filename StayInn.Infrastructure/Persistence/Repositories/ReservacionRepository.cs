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

        public async Task<IEnumerable<Reservacion>> BuscarPorFechaAsync(DateOnly? fechaEntrada, DateOnly? fechaSalida, int pagina, int tamanoPagina)
        {
            var consulta = _context.Reservaciones
                .Include(r => r.Habitacion)
                .Include(r => r.Usuario)
                .AsNoTracking()
                .AsQueryable();

            if (fechaEntrada.HasValue)
            {
                consulta = consulta.Where(r => r.FechaEntrada >= fechaEntrada.Value);
            }

            if (fechaSalida.HasValue)
            {
                consulta = consulta.Where(r => r.FechaSalida <= fechaSalida.Value);
            }

            return await consulta
                .OrderByDescending(r => r.FechaEntrada)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }

        public async Task<int> ContarBuscarPorFechasAsync(DateOnly? fechaEntrada, DateOnly? fechaSalida)
        {
            var consulta = _context.Reservaciones.AsQueryable();

            if (fechaEntrada.HasValue)
            {
                consulta = consulta.Where(r => r.FechaEntrada >= fechaEntrada.Value);
            }

            if (fechaSalida.HasValue)
            {
                consulta = consulta.Where(r => r.FechaSalida <= fechaSalida.Value);
            }

            return await consulta.CountAsync();
        }

        public async Task<int> ContarFiltroAsync(string valor)
        {
            var valorNormalizado = valor.Trim().ToLower();
            var pattern = $"%{valorNormalizado}%";

            return await _context.Reservaciones
                .Where(r =>
                    EF.Functions.Like(r.Usuario.NombreCompleto.ToLower(), pattern) ||
                    EF.Functions.Like(r.Habitacion.Numero.ToLower(), pattern))
                .CountAsync();
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

        public async Task<IEnumerable<Reservacion>> FiltrarReservacionAsync(string valor, int pagina, int tamanoPagina)
        {
            var valorNormalizado = valor.Trim().ToLower();
            var pattern = $"%{valorNormalizado}%";

            return await _context.Reservaciones
                .Include(r => r.Habitacion)
                .Include(r => r.Usuario)
                .Where(r =>
                    EF.Functions.Like(r.Usuario.NombreCompleto.ToLower(), pattern) ||
                    EF.Functions.Like(r.Habitacion.Numero.ToLower(), pattern))
                .OrderByDescending(r => r.FechaEntrada)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
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
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.FechaEntrada)
            .ThenByDescending(r => r.Estado == EstadoReservacion.Pendiente)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();

        public async Task<IEnumerable<Reservacion>> ObtenerUltimasCincoAsync()
        => await _context.Reservaciones
            .Include(r => r.Habitacion)
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.Id) 
            .Take(5)                      
            .ToListAsync();
    }
}
