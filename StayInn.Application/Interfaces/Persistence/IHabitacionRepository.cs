using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IHabitacionRepository
    {
        Task<Habitacion?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Habitacion>> ObtenerTodasAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<Habitacion>> ObtenerDisponiblesAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<Habitacion>> BuscarHabitacion(string valor, int pagina, int tamanoPagina);
        Task<int> ContarTodasAsync();
        Task<int> ContarDisponiblesAsync();
        Task<int> ContarBusquedaAsync(string valor);
        Task<bool> ExisteNumeroHabitacionAsync(string numero);
        Task<bool> TienereservacionAsociada(int id);


        Task CrearAsync (Habitacion habitacion);
        Task ActualizarAsync (Habitacion habitacion);
        Task EliminarAsync (int id);
        Task CambiarEstadoAsync(Habitacion habitacion);
        Task<bool> TieneReservacionActivaAsync(int habitacionId);
    }
}