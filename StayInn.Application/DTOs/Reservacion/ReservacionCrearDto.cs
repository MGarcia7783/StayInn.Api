using System.ComponentModel.DataAnnotations;

namespace StayInn.Application.DTOs.Reservacion
{
    public class ReservacionCrearDto
    {
        [Required (ErrorMessage = "El ID del hotel es obligatorio.")]
        public int HotelId { get; set; }


        [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
        public DateTime FechaEntrada { get; set; }


        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        public DateTime FechaSalida { get; set; }
    }
}
