using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Application.DTOs.Dashboard
{
    public class ReservacionDashboardDto
    {
        public string NumeroHabitacion { get; set; } = null!;
        public string NombreHuesped { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
