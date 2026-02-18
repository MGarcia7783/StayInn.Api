using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInn.Application.DTOs.Usuario;
using StayInn.Application.Interfaces.Service;
using StayInn.Application.Response;

namespace StayInn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ObtenerTodos([FromQuery] int numeroPagina = 1, [FromQuery] int tamanoPagina = 10)
        {
            var usuarios = await _service.ObtenerUsuariosAsync(numeroPagina, tamanoPagina);
            var totalUsuarios = await _service.ContarAsycn();

            return Ok(new RespuestaPaginada<UsuarioDto>(usuarios, totalUsuarios, numeroPagina, tamanoPagina));
        }


        [HttpGet("{id}", Name = "ObtenerUsuario")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerPorId(string id)
        {
            var usuario = await _service.ObtenerUsuarioPorIdAsync(id);

            return Ok(usuario);
        }


        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<ActionResult> RegistrarUsuario([FromBody] UsuarioRegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.RegistrarUsuario(dto);
            return CreatedAtRoute("ObtenerUsuario", new { id = resultado.Id }, resultado);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginRespuestaUsuarioDto>> Login([FromBody] UsuarioLoginDto dto)
        {
            return await _service.LoginAsync(dto);
        }


        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> CambiarEstado(string id, [FromBody] bool estado)
        {
            await _service.CambiarEstadoAsync(id, estado);
            return NoContent();
        }
    }
}
