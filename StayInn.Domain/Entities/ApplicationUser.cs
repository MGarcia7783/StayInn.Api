using Microsoft.AspNetCore.Identity;

namespace StayInn.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = null!;
    
            public ICollection<Reservacion> Reservaciones { get; set; } = new List<Reservacion>(); // Navegación
    }
}
