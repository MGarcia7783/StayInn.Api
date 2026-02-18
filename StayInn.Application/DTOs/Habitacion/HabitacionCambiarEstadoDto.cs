using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StayInn.Application.DTOs.Habitacion
{
    public class HabitacionCambiarEstadoDto
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public bool EstaDisponible { get; set; }

    }
}
