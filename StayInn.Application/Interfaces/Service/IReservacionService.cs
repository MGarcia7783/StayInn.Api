using StayInn.Application.DTOs.Reservacion;
using StayInn.Domain.Enums;

namespace StayInn.Application.Interfaces.Service
{
    public interface IReservacionService
    {
        Task<IEnumerable<ReservacionDto>> ObtenerTodasAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<ReservacionDto>> BuscarPorFechasAsync(DateOnly? fechaEntrada, DateOnly? fechaSalida, int pagina, int tamanoPagina);
        Task<IEnumerable<ReservacionDto>> FiltrarReservacionAsync(string valor,  int pagina, int tamanoPagina);
        Task<int> ContarTodasAsync();
        Task<int> ContarBuscarPorFechasAsync(DateOnly? fechaEntrada, DateOnly? fechaSalida);
        Task<int> ContarFiltroAsync(string valor);
        Task<IEnumerable<ReservacionDto>> ObtenerPorUsuarioAsync(string usuario);
        Task<ReservacionDto> ObtenerPorIdAsync(int id);


        Task<ReservacionDto> CrearAsync(ReservacionCrearDto dto, string usuarioId);
        Task<bool> CambiarEstadoAsync(int reservacionId, EstadoReservacion nuevoEstado);
        Task<ReservacionDto> CambiarFechaSalidaAsync(ReservacionCambiarFechaSalidaDto dto);
        Task<bool> CancelarAsync(int id);
    }
}
