
namespace StayInn.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        // 1. Métricas Clave
        public int TotalHabitaciones { get; set; }
        public int HabitacionesOcupadas { get; set; }
        public int ReservasPendientes { get; set; }
        public decimal IngresosMesActual { get; set; }

        // 2. Logística de hoy
        public List<ReservacionDashboardDto> LlegadasHoy { get; set; } = new();
        public List<ReservacionDashboardDto> SalidasHoy { get; set; } = new();

        // 3. Gráfico de tendencias (7 días)
        public List<TendenciaDto> TendenciaReservas { get; set; } = new();

        // 4. Actividad reciente
        public List<ActividadDto> Actividades { get; set; } = new();
    }
}
