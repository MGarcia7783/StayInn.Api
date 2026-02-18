
using StayInn.Application.DTOs.Habitacion;

namespace StayInn.Application.Interfaces.Service
{
    public interface IHabitacionService
    {
        Task<HabitacionDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<HabitacionDto>> ObtenerTodasAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<HabitacionDto>> ObtenerDisponiblesAsync(int pagina, int tamanoPagina);
        Task<int> ContarTodasAsync();
        Task<int> ContarDisponiblesAsync();


        Task<HabitacionDto> CrearAsync(HabitacionCrearDto dto);
        Task<HabitacionDto> ActualizarAsync(int id, HabitacionActualizarDto dto);
        Task EliminarAsync(int id);
        Task CambiarEstadoAsync(int id, bool disponible);
    }
}