
namespace StayInn.Application.DTOs.Reservacion
{
    public class ReservacionDto
    {
        public int Id { get; set; }
        public DateOnly FechaEntrada { get; set; }
        public DateOnly FechaSalida { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; } = null!;
        public string NumeroHabitacion { get; set; } = null!;
    }
}
