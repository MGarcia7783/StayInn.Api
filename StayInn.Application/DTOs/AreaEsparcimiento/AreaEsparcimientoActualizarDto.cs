
using System.ComponentModel.DataAnnotations;

namespace StayInn.Application.DTOs.AreaEsparcimiento
{
    public class AreaEsparcimientoActualizarDto
    {
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? ImagenUrl { get; set; }
    }
}
