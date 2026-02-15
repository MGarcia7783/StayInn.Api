using StayInn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IAreaEsparcimientoRepository
    {
        Task<IEnumerable<AreaEsparcimiento>> GetHomeAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<IEnumerable<AreaEsparcimiento>> GetAllAsync();


        Task<AreaEsparcimiento?> GetByIdAsync(int id);
        Task CreateAsync(AreaEsparcimiento areaEsparcimiento);
        Task UpdateAsync(AreaEsparcimiento areaEsparcimiento);
        Task DeleteAsync(int id);
    }
}
