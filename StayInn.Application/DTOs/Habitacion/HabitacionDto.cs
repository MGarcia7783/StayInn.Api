
namespace StayInn.Application.DTOs.Habitacion
{
    public class HabitacionDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal PrecioNoche { get; set; }
        public bool EstaDisponible { get; set; }
        public int CapacidadMax { get; set; }
    }
}
