using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Infrastructure.Persistence.Data;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
            => await _context.Users.CountAsync();

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(int page, int pageSize)
            => await _context.Users
                .Skip((page - 1) * pageSize)
                .OrderBy(u => u.NombreCompleto)
                .Take(pageSize)
                .ToListAsync();

        public async Task<ApplicationUser?> GetByIdAsync(string id)
            => await _context.Users.FindAsync(id);
    }
}
