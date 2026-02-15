using System.ComponentModel.DataAnnotations;

namespace StayInn.Application.DTOs.Habitacion
{
    public class HabitacionCrearDto
    {
        public int HotelId { get; set; }
        public string Numero { get; set; } = null!;
        public int CapacidadMax { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal PrecioNoche { get; set; }
        public string ImagenUrl { get; set; } = null!;
    }
}
