using StayInn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IUsuarioRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync(int page, int pageSize);
        Task<int> CountAsync();
    }
}
