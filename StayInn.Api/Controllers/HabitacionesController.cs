using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInn.Application.DTOs.Habitacion;
using StayInn.Application.DTOs.Hotel;
using StayInn.Application.Interfaces.Service;
using StayInn.Application.Response;

namespace StayInn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionesController : ControllerBase
    {
        private readonly IHabitacionService _service;

        public HabitacionesController(IHabitacionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<HabitacionDto>>> ObtenerTodasHabitaciones([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10)
        {
            var habitaciones = await _service.ObtenerTodasAsync(pagina, tamanoPagina);
            var total = await _service.ContarTodasAsync();

            return Ok(new RespuestaPaginada<HabitacionDto>(habitaciones, total, pagina, tamanoPagina));
        }


        [HttpGet("disponibles")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HabitacionDto>>> ObtenerHabitacionesDisponibles([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10)
        {
            var habitaciones = await _service.ObtenerDisponiblesAsync(pagina, tamanoPagina);
            var total = await _service.ContarDisponiblesAsync();

            return Ok(new RespuestaPaginada<HabitacionDto>(habitaciones, total, pagina, tamanoPagina));
        }


        [HttpGet("{id:int}", Name = "GetHabitacion")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHabitacion(int id)
        {
            var habitacion = await _service.ObtenerPorIdAsync(id);
            return Ok(habitacion);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<HabitacionDto>> Crear([FromBody] HabitacionCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var habitacion = await _service.CrearAsync(dto);
            return CreatedAtAction("GetHabitacion", new { id = habitacion.Id }, habitacion);
        }


        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<HabitacionDto>> Actualizar(int id, [FromBody] HabitacionActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.ActualizarAsync(id, dto);
            return NoContent();
        }


        [HttpPut("{id:int}/estado")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> CambiarEstado(int id, [FromBody] HabitacionCambiarEstadoDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CambiarEstadoAsync(dto.Id, dto.EstaDisponible);
            return NoContent();

        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }
    }
}
