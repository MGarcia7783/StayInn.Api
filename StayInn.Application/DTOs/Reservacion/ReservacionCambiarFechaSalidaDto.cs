using System.ComponentModel.DataAnnotations;

namespace StayInn.Application.DTOs.Reservacion
{
    public class ReservacionCambiarFechaSalidaDto
    {
        [Required(ErrorMessage = "El ID de la reservación es obligatorio.")]
        public int ReservacionId { get; set; }


        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        public DateOnly NuevaFechaSalida { get; set; }
    }
}
