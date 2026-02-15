using Microsoft.EntityFrameworkCore;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Entities;
using StayInn.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Hotel hotel)
        {
            _context.Hotel.Add(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task<Hotel?> GetAsync()
            => await _context.Hotel.FirstOrDefaultAsync();

        public Task UpdateAsync(Hotel hotel)
        {
            _context.Hotel.Update(hotel);
            return _context.SaveChangesAsync();
        }
    }
}
