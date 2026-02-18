using AutoMapper;
using StayInn.Application.DTOs.Hotel;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;
using StayInn.Domain.Entities;

namespace StayInn.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _repository;
        private readonly IImageStorageService _imageStorageService;
        private readonly IMapper _mapper;

    public HotelService(IHotelRepository repository, IImageStorageService imageStorageService,  IMapper mapper)
        {
            _repository = repository;
            _imageStorageService = imageStorageService;
            _mapper = mapper;
        }

        public async Task<HotelDto> ActualizarAsync(HotelActualizarDto dto, string? nuevaImagenUrl)
        {
            var hotel = await _repository.ObtenerAsync();
            if (hotel == null)
                throw new InvalidOperationException("No hay un hotel registrado para actualizar.");

            var imagenAnterior = hotel.ImagenPrincipal;

            // Si se proporcionó una nueva imagen, actualizamos la URL
            if (!string.IsNullOrEmpty(nuevaImagenUrl))
            {
                hotel.ImagenPrincipal = nuevaImagenUrl;
            }

            // Mapear el resto de los campos (sin tocar la imagen si no se proporcionó una nueva)
            _mapper.Map(dto, hotel);

            try
            {
                await _repository.ActualizarAsync(hotel);

                // OK → eliminar imagen anterior
                if (!string.IsNullOrEmpty(nuevaImagenUrl) &&
                    !string.IsNullOrEmpty(imagenAnterior))
                    await _imageStorageService.EliminarImagenAsync(imagenAnterior);

                return _mapper.Map<HotelDto>(hotel);
            }
            catch
            {
                // Falló → eliminar imagen nueva
                if (!string.IsNullOrEmpty(nuevaImagenUrl))
                    await _imageStorageService.EliminarImagenAsync(nuevaImagenUrl);

                throw;
            }
        }

        public async Task<HotelDto> CrearAsync(HotelCrearDto dto, string imageUrl)
        {
            var existente = await _repository.ObtenerAsync();
            if (existente != null)
                throw new InvalidOperationException("El hotel ya está registrado.");

            if (imageUrl == null)
                throw new InvalidOperationException("Se requiere una imagen.");

            var hotel = _mapper.Map<Hotel>(dto);
            hotel.ImagenPrincipal = imageUrl;

            try
            {
                await _repository.CrearAsync(hotel);
                return _mapper.Map<HotelDto>(hotel);
            }
            catch
            {
                // Si ocurre un error, eliminamos la imagen que se subió para evitar archivos huérfanos
                if (!string.IsNullOrEmpty(imageUrl))
                    await _imageStorageService.EliminarImagenAsync(imageUrl);

                throw;
            }
        }

        public async Task<HotelDto?> ObtenerAsync()
        {
            var hotel = await _repository.ObtenerAsync();
            return hotel == null ? null : _mapper.Map<HotelDto>(hotel);
        }
    }
}
