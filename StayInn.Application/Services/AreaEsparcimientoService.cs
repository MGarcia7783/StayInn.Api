using AutoMapper;
using StayInn.Application.DTOs.AreaEsparcimiento;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;
using StayInn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Application.Services
{
    public class AreaEsparcimientoService : IAreaEsparcimientoService
    {
        private readonly IAreaEsparcimientoRepository _repository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IImageStorageService _imageStorageService;
        private readonly IMapper _mapper;

        public AreaEsparcimientoService (IAreaEsparcimientoRepository repository, IImageStorageService imageStorageService, IMapper mapper, IHotelRepository hotelRepository)
        {
            _repository = repository;
            _imageStorageService = imageStorageService;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        public async Task<AreaEsparcimientoDto> ActualizarAsync(int id, AreaEsparcimientoActualizarDto dto, string? nuevaImagenUrl)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new InvalidOperationException("Registro no encontrado.");

            var imagenAnterior = registro.ImagenUrl;

            _mapper.Map(dto, registro);

            // Si se proporcionó una nueva imagen, actualizamos la URL
            if (!string.IsNullOrEmpty(nuevaImagenUrl))
                registro.ImagenUrl = nuevaImagenUrl;

            try
            {
                await _repository.ActualizarAsync(registro);

                if (!string.IsNullOrEmpty(nuevaImagenUrl) && !string.IsNullOrEmpty(imagenAnterior))
                    await _imageStorageService.EliminarImagenAsync(imagenAnterior);

                return _mapper.Map<AreaEsparcimientoDto>(registro);
            }
            catch
            {
                if(!string.IsNullOrEmpty(nuevaImagenUrl))
                    await _imageStorageService.EliminarImagenAsync(nuevaImagenUrl);

                throw;
            }
        }

        public async Task<AreaEsparcimientoDto> CrearAsync(AreaEsparcimientoCrearDto dto, string imagenUrl)
        {
            if (imagenUrl == null)
                throw new InvalidOperationException("Se requiere una imagen.");

            // Obtener el id del hotel
            var hotel = await _hotelRepository.ObtenerAsync();
            if (hotel == null)
                throw new InvalidOperationException("No existe un hotel registrado.");

            var registro = _mapper.Map<AreaEsparcimiento>(dto);
            registro.ImagenUrl = imagenUrl;
            registro.HotelId = hotel.Id;

            try
            {
                await _repository.CrearAsync(registro);
                return _mapper.Map<AreaEsparcimientoDto>(registro);
            }
            catch
            {
                if (!string.IsNullOrEmpty(imagenUrl))
                    await _imageStorageService.EliminarImagenAsync(imagenUrl);

                throw;
            }
        }

        public async Task EliminarAsync(int id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException("Registro no encontrado");

            await _imageStorageService.EliminarImagenAsync(registro.ImagenUrl);
            await _repository.EliminarAsync(id);
        }

        public async Task<IEnumerable<AreaEsparcimientoHomeDto>> ObtenerInicioAsync()
        {
            var registros = await _repository.ObtenerInicioAsync();
            return _mapper.Map<IEnumerable<AreaEsparcimientoHomeDto>>(registros);
        }

        public async Task<AreaEsparcimientoDto?> ObtenerPorIdAsync(int id)
        {
            if(id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero.");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException($"No se encontró el registro con ID: '{id}'.");

            return _mapper.Map<AreaEsparcimientoDto>(registro);
        }

        public async Task<IEnumerable<AreaEsparcimientoDto>> ObtenerTodasAsync()
        {
            var registros = await _repository.ObtenerTodasAsync();
            return _mapper.Map<IEnumerable<AreaEsparcimientoDto>>(registros);
        }
    }
}
