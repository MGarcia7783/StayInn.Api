
namespace StayInn.Domain.Entities
{
    public class Habitacion
    {
        public int Id { get; set; }

        public string Numero { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal PrecioNoche { get; set; }
        public string ImagenUrl { get; set; } = null!;
        public bool EstaDisponible { get; set; }
        public int CapacidadMax { get; set; }


        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;   // Navegación
        public ICollection<Reservacion> Reservaciones { get; set; } = new List<Reservacion>(); // Navegación
    }
}
