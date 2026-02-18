
namespace StayInn.Application.DTOs.Hotel
{
    public class HotelActualizarDto
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string? ImagenPrincipal { get; set; }
    }
}
