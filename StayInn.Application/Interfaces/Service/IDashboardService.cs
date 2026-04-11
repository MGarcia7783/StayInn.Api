using StayInn.Application.DTOs.Dashboard;

namespace StayInn.Application.Interfaces.Service
{
    public interface IDashboardService
    {
        Task<DashboardDto> ObtenerResumenAsync();
    }
}
