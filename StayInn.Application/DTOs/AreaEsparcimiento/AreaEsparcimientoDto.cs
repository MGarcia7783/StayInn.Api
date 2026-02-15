
namespace StayInn.Application.DTOs.AreaEsparcimiento
{
    public class AreaEsparcimientoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string ImagenUrl { get; set; } = null!;
        public int HotelId { get; set; }
    }
}
