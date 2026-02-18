using StayInn.Application.DTOs.AreaEsparcimiento;

namespace StayInn.Application.Interfaces.Service
{
    public interface IAreaEsparcimientoService
    {
        Task<IEnumerable<AreaEsparcimientoHomeDto>> ObtenerInicioAsync();
        Task<IEnumerable<AreaEsparcimientoDto>> ObtenerTodasAsync();


        Task<AreaEsparcimientoDto?> ObtenerPorIdAsync(int id);
        Task<AreaEsparcimientoDto> CrearAsync(AreaEsparcimientoCrearDto dto, string imagenUrl);
        Task<AreaEsparcimientoDto> ActualizarAsync(int id, AreaEsparcimientoActualizarDto dto, string? nuevaImagenUrl);
        Task EliminarAsync(int id);
    }
}
