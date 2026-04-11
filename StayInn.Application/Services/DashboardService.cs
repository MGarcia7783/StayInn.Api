using StayInn.Application.DTOs.Dashboard;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;

namespace StayInn.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        private readonly IReservacionRepository _resRepository;

        public DashboardService(IDashboardRepository repository, IReservacionRepository resRepository)
        {
            _repository = repository;
            _resRepository = resRepository;
        }

        public async Task<DashboardDto> ObtenerResumenAsync()
        {
            var dto = await _repository.ObtenerDatosDashboardAsync();

            var ultimasRes = await _resRepository.ObtenerUltimasCincoAsync();

            dto.Actividades = ultimasRes.Select(r => new ActividadDto
            {
                Tipo = "Reserva",
                Descripcion = $"Nueva reservación Hab. {r.Habitacion.Numero} por {r.Usuario.NombreCompleto}",
                Tiempo = CalcularTiempoRelativo(r.FechaRegistro)
            }).ToList();

            return dto;
        }

        private string CalcularTiempoRelativo(DateOnly fecha)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            if (fecha == hoy) return "Hoy";
            if (fecha == hoy.AddDays(-1)) return "Ayer";
            return fecha.ToString("dd/MM/yyyy");
        }
    }
}
