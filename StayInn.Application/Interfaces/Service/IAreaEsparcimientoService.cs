using StayInn.Application.DTOs.AreaEsparcimiento;
using StayInn.Application.DTOs.Habitacion;

namespace StayInn.Application.Interfaces.Service
{
    public interface IAreaEsparcimientoService
    {
        Task<IEnumerable<AreaEsparcimientoHomeDto>> ObtenerInicioAsync();
        Task<IEnumerable<AreaEsparcimientoDto>> ObtenerTodasAsync();
        Task<IEnumerable<AreaEsparcimientoDto>> BuscarAreaEsparcimientoAsync(string nombre);
        Task<AreaEsparcimientoDto?> ObtenerPorIdAsync(int id);


        Task<AreaEsparcimientoDto> CrearAsync(AreaEsparcimientoCrearDto dto, string imagenUrl);
        Task<AreaEsparcimientoDto> ActualizarAsync(int id, AreaEsparcimientoActualizarDto dto, string? nuevaImagenUrl);
        Task EliminarAsync(int id);
    }
}
