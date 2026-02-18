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

        public async Task<int> ContarAsync()
            => await _context.Users.CountAsync();

        public async Task<ApplicationUser?> ObtenerPorIdAsync(string id)
            => await _context.Users.FindAsync(id);

        public async Task<IEnumerable<ApplicationUser>> ObtenerTodosAsync(int pagina, int tamanoPagina)
            => await _context.Users
                .Skip((pagina - 1) * tamanoPagina)
                .OrderBy(u => u.NombreCompleto)
                .Take(tamanoPagina)
                .ToListAsync();
    }
}
