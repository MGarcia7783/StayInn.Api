
namespace StayInn.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        // Navegación
        public ICollection<AreaEsparcimiento> AreasEsparcimiento { get; set; } = new List<AreaEsparcimiento>();
        public ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
    }
}
