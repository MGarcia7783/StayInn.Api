using StayInn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetAsync();
        Task CreateAsync (Hotel hotel);
        Task UpdateAsync (Hotel hotel);
    }
}
