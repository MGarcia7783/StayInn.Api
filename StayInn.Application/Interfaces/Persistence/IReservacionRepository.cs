using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IReservacionRepository
    {
        Task<Reservacion?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Reservacion>> ObtenerTodasAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<Reservacion>> BuscarPorFechaAsync(DateOnly? fechaEntrada, DateOnly? fechaSalida, int pagina, int tamanoPagina);
        Task<IEnumerable<Reservacion>> FiltrarReservacionAsync(string valor, int pagina, int tamanoPagina);
        Task<int> ContarTodasAsync(); 
        Task<int> ContarBuscarPorFechasAsync(DateOnly? fechaEntrada, DateOnly? fechaSalida); 
        Task<int> ContarFiltroAsync(string valor); 
        Task<IEnumerable<Reservacion>> ObtenerPorUsuarioAsync(string usuarioId);
        Task<bool> ExisteReservacionActivaEnRangoAsync(int habitacionId, DateOnly fechaEntrada, DateOnly fechaSalida, int? reservacionId = null);
        Task<IEnumerable<Reservacion>> ObtenerUltimasCincoAsync();



        Task CrearAsync(Reservacion reservacion);
        Task ActualizarAsync(Reservacion reservacion);
        Task<int> GuardarCambiosAsync();
    }
}