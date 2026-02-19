using StayInn.Domain.Enums;

namespace StayInn.Domain.Entities
{
    public class Reservacion
    {
        public int Id { get; set; }

        public DateOnly FechaEntrada { get; set; }
        public DateOnly FechaSalida { get; set; } 
        public decimal MontoTotal { get; set; }
        public EstadoReservacion Estado { get; set; } = EstadoReservacion.Pendiente;


        public int HabitacionId { get; set; }
        public Habitacion Habitacion { get; set; } = null!; // Navegación

        public string UsuarioId { get; set; } = null!;
        public ApplicationUser Usuario { get; set; } = null!; // Navegación
    }
}
