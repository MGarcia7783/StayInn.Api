using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayInn.Api.Request.Hotel;
using StayInn.Application.DTOs.Hotel;
using StayInn.Application.Interfaces.Service;

namespace StayInn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _service;
        private readonly IImageStorageService _imageService;
        private readonly IMapper _mapper;

        public HotelController(IHotelService service, IImageStorageService imageService, IMapper mapper)
        {
            _service = service;
            _imageService = imageService;
            _mapper = mapper;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ObtenerHotel()
        {
            var hotels = await _service.ObtenerAsync();
            if (hotels == null)
                return NotFound(new { message = "No hay información registrada." });

            return Ok(hotels);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<HotelDto>> Crear([FromForm] HotelCrearRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Subir imagen
            var imagenUrl = await _imageService.SubirImagenAsync(
                request.ImagenPrincipal.OpenReadStream(),
                request.ImagenPrincipal.FileName,
                request.ImagenPrincipal.ContentType,
                folder: "hotel"
            );

            // Mapear Request → DTO
            var dto = new HotelCrearDto
            {
                Nombre = request.Nombre,
                Email = request.Email,
                Telefono = request.Telefono,
                Descripcion = request.Descripcion,
                Direccion = request.Direccion,
                Latitud = request.Latitud,
                Longitud = request.Longitud
            };

            // Servicio maneja todo (incluido rollback)
            var hotel = await _service.CrearAsync(dto, imagenUrl);

            return CreatedAtAction(nameof(ObtenerHotel), new { id = hotel.Id }, hotel);
        }


        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<HotelDto>> Actualizar([FromForm] HotelActualizarRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? nuevaImagenUrl = null;

            // Subir imagen solo si viene
            if (request.ImagenPrincipal != null)
            {
                nuevaImagenUrl = await _imageService.SubirImagenAsync(
                    request.ImagenPrincipal.OpenReadStream(),
                    request.ImagenPrincipal.FileName,
                    request.ImagenPrincipal.ContentType,
                    folder: "hotel"
                );
            }

            var dto = new HotelActualizarDto
            {
                Nombre = request.Nombre,
                Email = request.Email,
                Telefono = request.Telefono,
                Descripcion = request.Descripcion,
                Direccion = request.Direccion,
                Latitud = request.Latitud,
                Longitud = request.Longitud
            };

            var hotel = await _service.ActualizarAsync(dto, nuevaImagenUrl);
            return NoContent();
        }
    }
}
