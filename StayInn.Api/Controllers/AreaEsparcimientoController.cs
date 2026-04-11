using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInn.Api.Request.AreaEsparcimiento;
using StayInn.Application.DTOs.AreaEsparcimiento;
using StayInn.Application.DTOs.Habitacion;
using StayInn.Application.Interfaces.Service;
using StayInn.Application.Response;

namespace StayInn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaEsparcimientoController : ControllerBase
    {
        private readonly IAreaEsparcimientoService _service;
        private readonly IImageStorageService _imageService;
        private readonly IMapper _mapper;

        public AreaEsparcimientoController(IAreaEsparcimientoService service, IImageStorageService imageService, IMapper mapper)
        {
            _service = service;
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AreaEsparcimientoDto>>> ObtenerTodas()
        {
            var registros = await _service.ObtenerTodasAsync();
            return Ok(registros);
        }


        [HttpGet("home")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AreaEsparcimientoHomeDto>>> ObtenerInicio()
        {
            var registros = await _service.ObtenerInicioAsync();
            return Ok(registros);
        }


        [HttpGet("{id:int}", Name = "GetAreaEsparcimiento")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAreaEsparcimiento(int id)
        {
            var registro = await _service.ObtenerPorIdAsync(id);
            return Ok(registro);
        }

        [HttpGet("buscar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Buscar([FromQuery] string nombre)
        {
            var areaEsparcmiento = await _service.BuscarAreaEsparcimientoAsync(nombre);

            return Ok(areaEsparcmiento);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<AreaEsparcimientoDto>> Crear([FromForm] AreaEsparcimientoCrearRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagenUrl = await _imageService.SubirImagenAsync(
                request.ImagenUrl.OpenReadStream(),
                request.ImagenUrl.FileName,
                request.ImagenUrl.ContentType,
                folder: "esparcimiento"
            );

            var dto = new AreaEsparcimientoCrearDto
            {
                Nombre = request.Nombre
                //HotelId = request.HotelId,
            };

            var registro = await _service.CrearAsync(dto, imagenUrl);
            return CreatedAtAction("GetAreaEsparcimiento", new { id = registro.Id }, registro);
        }


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<AreaEsparcimientoDto>> Actualizar(int id, [FromForm] AreaEsparcimientoActualizarRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? nuevaImagenUrl = null;

            if (request.ImagenUrl != null)
            {
                nuevaImagenUrl = await _imageService.SubirImagenAsync(
                    request.ImagenUrl.OpenReadStream(),
                    request.ImagenUrl.FileName,
                    request.ImagenUrl.ContentType,
                    folder: "esparcimiento"
                );
            }

            var dto = new AreaEsparcimientoActualizarDto
            {
                Nombre = request.Nombre
            };

            var registro = await _service.ActualizarAsync(id, dto, nuevaImagenUrl);
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
