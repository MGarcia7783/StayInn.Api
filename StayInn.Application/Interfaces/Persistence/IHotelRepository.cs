using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IHotelRepository
    {
        Task<Hotel?> ObtenerAsync();


        Task CrearAsync (Hotel hotel);
        Task ActualizarAsync (Hotel hotel);
    }
}