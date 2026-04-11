using StayInn.Domain.Entities;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IAreaEsparcimientoRepository
    {
        Task<IEnumerable<AreaEsparcimiento>> ObtenerInicioAsync();
        Task<IEnumerable<AreaEsparcimiento>> ObtenerTodasAsync();
        Task<IEnumerable<AreaEsparcimiento>> BuscarAreaEsparcimiento(string nombre);


        Task<AreaEsparcimiento?> ObtenerPorIdAsync(int id);
        Task CrearAsync(AreaEsparcimiento areaEsparcimiento);
        Task ActualizarAsync(AreaEsparcimiento areaEsparcimiento);
        Task EliminarAsync(int id);
    }
}

