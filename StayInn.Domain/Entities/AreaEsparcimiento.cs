
namespace StayInn.Domain.Entities
{
    public class AreaEsparcimiento
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string ImagenUrl { get; set; } = null!;

        
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;   // Navegación
    }
}
