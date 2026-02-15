
namespace StayInn.Application.DTOs.Reservacion
{
    public class ReservacionResponseDto
    {
        public int Id { get; set; }
        public DateTimeOffset FechaEntrada { get; set; }
        public DateTimeOffset FechaSalida { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; } = null!;
        public string NumeroHabitacion { get; set; } = null!;
    }
}
