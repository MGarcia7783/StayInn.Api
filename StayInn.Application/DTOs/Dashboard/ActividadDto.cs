using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Application.DTOs.Dashboard
{
    public class ActividadDto
    {
        public string Descripcion { get; set; } = null!;
        public string Tiempo { get; set; } = null!; // Ejemplo: "Hace 2 horas"
        public string Tipo { get; set; } = null!; // Reserva, Usuario, Habitacion
    }
}
