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

        public async Task<IEnumerable<ApplicationUser>> BuscarUsuarioAsync(string valor, int pagina, int tamanoPagina)
        {
            var valorNormalizado = valor.Trim().ToLower();
            var pattern = $"%{valorNormalizado}%";

            return await _context.Users
                .Where(u =>
                    EF.Functions.Like(u.NombreCompleto.ToLower(), pattern) ||
                    EF.Functions.Like(u.Email!.ToLower(), pattern) ||
                    EF.Functions.Like(u.PhoneNumber!.ToLower(), pattern))
                .OrderBy(u => u.NombreCompleto)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }

        public async Task<int> ContarAsync()
            => await _context.Users.CountAsync();

        public async Task<int> ContarBusquedaAsync(string valor)
        {
            var valorNormalizado = valor.Trim().ToLower();
            var pattern = $"%{valorNormalizado}%";

            return await _context.Users
                .Where(u =>
                    EF.Functions.Like(u.NombreCompleto.ToLower(), pattern) ||
                    EF.Functions.Like(u.Email!.ToLower(), pattern) ||
                    EF.Functions.Like(u.PhoneNumber!.ToLower(), pattern))
                .CountAsync();
        }

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
