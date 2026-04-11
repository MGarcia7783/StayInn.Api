using StayInn.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Application.Interfaces.Persistence
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> ObtenerDatosDashboardAsync();
    }
}
