

using AutoMapper;
using StayInn.Application.DTOs.Habitacion;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;
using StayInn.Domain.Entities;

namespace StayInn.Application.Services
{
    public class HabitacionService : IHabitacionService
    {
        private readonly IHabitacionRepository _repository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HabitacionService(IHabitacionRepository repository, IHotelRepository hotelRepository, IMapper mapper)
        {
            _repository = repository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<HabitacionDto> ActualizarAsync(int id, HabitacionActualizarDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos de la solicitud no pueden ser nulos.");

            var habitacion = await _repository.ObtenerPorIdAsync(dto.Id);
            if (habitacion == null)
                throw new KeyNotFoundException("La habitación no existe.");

            // Validar duplicados
            var nuevoNumero = dto.Numero.Trim();
            var numeroActual = habitacion.Numero.Trim();

            if (!string.Equals(numeroActual, nuevoNumero, StringComparison.OrdinalIgnoreCase))
            {
                if(await _repository.ExisteNumeroHabitacionAsync(dto.Numero))
                    throw new InvalidOperationException($"Ya existe una habitación con el número: '{dto.Numero}'.");
            }

            _mapper.Map(dto, habitacion);
            await _repository.ActualizarAsync(habitacion);

            return _mapper.Map<HabitacionDto>(habitacion);
        }

        public async Task CambiarEstadoAsync(int id, bool disponible)
        {

            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero.");

            var habitacion = await _repository.ObtenerPorIdAsync(id);
            if (habitacion == null)
                throw new KeyNotFoundException($"No se encontró la habitación con ID: '{id}'.");


            // Regla de negocio: NO permitir marcar como disponible si tiene reservas activas

            if (disponible)
            {
                var tieneReservacionActiva =
                    await _repository.TieneReservacionActivaAsync(id);

                if (tieneReservacionActiva)
                    throw new InvalidOperationException("No se puede marcar la habitación como disponible porque tiene reservaciones activas.");
            }
            

            // Modificar la entidad
            habitacion.EstaDisponible = disponible;
            await _repository.CambiarEstadoAsync(habitacion);
        }

        public async Task<int> ContarDisponiblesAsync()
        {
            return await _repository.ContarDisponiblesAsync();
        }

        public async Task<int> ContarTodasAsync()
        {
            return await _repository.ContarTodasAsync();
        }

        public async Task<HabitacionDto> CrearAsync(HabitacionCrearDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos de la solicitud no pueden ser nulos.");

            var nuevoNumero = dto.Numero.Trim();
            if(await _repository.ExisteNumeroHabitacionAsync(nuevoNumero))
                throw new InvalidOperationException($"Ya existe una habitación con el número: '{dto.Numero}'.");

            // Obtener el hotelId
            var hotel = await _hotelRepository.ObtenerAsync();
            if (hotel == null)
                throw new InvalidOperationException("Debe guardar primero la información del hotel.");

            // Crar entidad y guardar
            var habitacion = _mapper.Map<Habitacion>(dto);
            habitacion.HotelId = hotel.Id;
            habitacion.EstaDisponible = true;

            await _repository.CrearAsync(habitacion);

            return _mapper.Map<HabitacionDto>(habitacion);
        }

        public async Task EliminarAsync(int id)
        {
            var habitacion = await _repository.ObtenerPorIdAsync(id);
            if (habitacion == null)
                throw new KeyNotFoundException("La habitación no existe.");

            // Eliminar registro
            await _repository.EliminarAsync(id);
        }

        public async Task<IEnumerable<HabitacionDto>> ObtenerDisponiblesAsync(int pagina, int tamanoPagina)
        {
            var habitaciones = await _repository.ObtenerDisponiblesAsync(pagina, tamanoPagina);
            return _mapper.Map<IEnumerable<HabitacionDto>>(habitaciones);
        }

        public async Task<HabitacionDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero.");

            var habitacion = await _repository.ObtenerPorIdAsync(id);
            if (habitacion == null)
                throw new KeyNotFoundException($"No se encontró el registro con ID: '{id}'.");

            return _mapper.Map<HabitacionDto>(habitacion);
        }

        public async Task<IEnumerable<HabitacionDto>> ObtenerTodasAsync(int pagina, int tamanoPagina)
        {
            var habitaciones = await _repository.ObtenerTodasAsync(pagina, tamanoPagina);
            return _mapper.Map<IEnumerable<HabitacionDto>>(habitaciones);
        }
    }
}
