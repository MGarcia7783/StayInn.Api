using StayInn.Application.DTOs.Reservacion;
using StayInn.Domain.Enums;

namespace StayInn.Application.Interfaces.Service
{
    public interface IReservacionService
    {
        Task<IEnumerable<ReservacionDto>> ObtenerTodasAsync(int pagina, int tamanoPagina);
        Task<int> ContarTodasAsync();
        Task<IEnumerable<ReservacionDto>> ObtenerPorUsuarioAsync(string usuarioId);
        Task<ReservacionDto> ObtenerPorIdAsync(int id);


        Task<ReservacionDto> CrearAsync(ReservacionCrearDto dto, string usuarioId);
        Task<bool> CambiarEstadoAsync(int reservacionId, EstadoReservacion nuevoEstado);
        Task<ReservacionDto> CambiarFechaSalidaAsync(ReservacionCambiarFechaSalidaDto dto);
        Task<bool> CancelarAsync(int id);
    }
}
