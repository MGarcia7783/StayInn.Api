using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Infrastructure.Persistence.Data;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class AreaEsparciminetoRepository : IAreaEsparcimientoRepository
    {
        private readonly ApplicationDbContext _context;

        public AreaEsparciminetoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
            => await _context.AreasEsparcimiento.CountAsync();

        public async Task CreateAsync(AreaEsparcimiento areaEsparcimiento)
        {
            _context.AreasEsparcimiento.Add(areaEsparcimiento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
            => await _context.AreasEsparcimiento.Where(a => a.Id == id).ExecuteDeleteAsync();

        public async Task<IEnumerable<AreaEsparcimiento>> GetAllAsync()
            => await _context.AreasEsparcimiento.ToListAsync();

        public async Task<AreaEsparcimiento?> GetByIdAsync(int id)
            => await _context.AreasEsparcimiento.FindAsync(id);

        public async Task<IEnumerable<AreaEsparcimiento>> GetHomeAsync(int page, int pageSize)
            => await _context.AreasEsparcimiento
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task UpdateAsync(AreaEsparcimiento areaEsparcimiento)
        {
            _context.AreasEsparcimiento.Update(areaEsparcimiento);
            await _context.SaveChangesAsync();
        }
    }
}
