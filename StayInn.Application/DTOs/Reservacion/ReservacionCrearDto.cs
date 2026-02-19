using System.ComponentModel.DataAnnotations;

namespace StayInn.Application.DTOs.Reservacion
{
    public class ReservacionCrearDto
    {
        [Required (ErrorMessage = "El número de habitacións es obligatorio.")]
        public int HabitacionId { get; set; }


        [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
        public DateOnly FechaEntrada { get; set; }


        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        public DateOnly FechaSalida { get; set; }
    }
}
