using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayInn.Application.DTOs.Reservacion;
using StayInn.Application.Interfaces.Service;
using StayInn.Application.Response;
using StayInn.Domain.Enums;
using System.Security.Claims;

namespace StayInn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservacionController : ControllerBase
    {
        private readonly IReservacionService _service;

        public ReservacionController(IReservacionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ObtenerTodos([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10)
        {
            var reservaciones = await _service.ObtenerTodasAsync(pagina, tamanoPagina);
            var total = await _service.ContarTodasAsync();

            return Ok(new RespuestaPaginada<ReservacionDto>(reservaciones, total, pagina, tamanoPagina));
        }


        [HttpGet("mis-reservaciones")]
        public async Task<ActionResult<IEnumerable<ReservacionDto>>> MisReservaciones()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservaciones = await _service.ObtenerPorUsuarioAsync(userId!);
            return Ok(reservaciones);
        }


        [HttpGet("{id:int}", Name = "ObtenerReservacion")]
        public async Task<IActionResult> ObtenerReservacion(int id)
        {
            var reservacion = await _service.ObtenerPorIdAsync(id);
            return Ok(reservacion);
        }


        [HttpPost]
        public async Task<ActionResult<ReservacionDto>> Crear([FromBody] ReservacionCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservacion = await _service.CrearAsync(dto, userId);
            return CreatedAtAction("ObtenerReservacion", new { id = reservacion.Id }, reservacion);
        }


        [HttpPut("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] EstadoReservacion nuevoEstado)
        {
            await _service.CambiarEstadoAsync(id, nuevoEstado);
            return Ok(new { mensaje = "Estado actualizado correctamente." });
        }


        [HttpPut("cambiar-fecha")]
        public async Task<ActionResult> CambiarFecha([FromBody] ReservacionCambiarFechaSalidaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CambiarFechaSalidaAsync(dto);
            return NoContent();
        }


        [HttpPatch("{id:int}/cancelar")]
        public async Task<ActionResult> Cancelar(int id)
        {
            await _service.CancelarAsync(id);
            return NoContent();
        }
    }
}
