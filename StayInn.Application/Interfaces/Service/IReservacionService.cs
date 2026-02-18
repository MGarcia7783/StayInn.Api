using StayInn.Application.DTOs.Reservacion;

namespace StayInn.Application.Interfaces.Service
{
    public interface IReservacionService
    {
        Task<IEnumerable<ReservacionResponseDto>> ObtenerTodasAsync(int pagina, int tamanoPagina);
        Task<int> ContarTodasAsync();
        Task<IEnumerable<ReservacionResponseDto>> ObtenerPorUsuarioAsync(string usuarioId);


        Task<ReservacionResponseDto> CrearAsync(ReservacionCrearDto dto);
        Task<bool> CancelarAsync(int idReservacion);   
        Task<ReservacionResponseDto> CambiarFechaSalidaAsync(
            ReservacionCambiarFechaSalidaDto dto
        );
    }
}
