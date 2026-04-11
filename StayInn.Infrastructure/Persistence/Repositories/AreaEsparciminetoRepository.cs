using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Infrastructure.Persistence.Data;
using System.Drawing;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class AreaEsparciminetoRepository : IAreaEsparcimientoRepository
    {
        private readonly ApplicationDbContext _context;

        public AreaEsparciminetoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(AreaEsparcimiento areaEsparcimiento)
        {
            _context.AreasEsparcimiento.Update(areaEsparcimiento);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AreaEsparcimiento>> BuscarAreaEsparcimiento(string nombre)
        {
            var nombreNormalizado = nombre.Trim().ToLower();
            var pattern = $"%{nombreNormalizado}%";

            return await _context.AreasEsparcimiento
                .Where(h =>
                    EF.Functions.Like(h.Nombre.ToLower(), pattern))
                .OrderBy(h => h.Nombre)
                .ToListAsync();
        }

        public async Task CrearAsync(AreaEsparcimiento areaEsparcimiento)
        {
            _context.AreasEsparcimiento.Add(areaEsparcimiento);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
            => await _context.AreasEsparcimiento.Where(a => a.Id == id).ExecuteDeleteAsync();

        public async Task<IEnumerable<AreaEsparcimiento>> ObtenerInicioAsync()
            => await _context.AreasEsparcimiento.ToListAsync();

        public async Task<AreaEsparcimiento?> ObtenerPorIdAsync(int id)
            => await _context.AreasEsparcimiento.FindAsync(id);

        public async Task<IEnumerable<AreaEsparcimiento>> ObtenerTodasAsync()
            => await _context.AreasEsparcimiento.ToListAsync();
    }
}
