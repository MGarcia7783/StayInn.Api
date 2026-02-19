using AutoMapper;
using StayInn.Application.DTOs.Reservacion;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;
using StayInn.Domain.Entities;
using StayInn.Domain.Enums;

namespace StayInn.Application.Services
{
    public class ReservacionService : IReservacionService
    {
        private readonly IReservacionRepository _repository;
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly IMapper _mapper;

        public ReservacionService(IReservacionRepository repository, IHabitacionRepository habitacionRepository, IMapper mapper)
        {
            _repository = repository;
            _habitacionRepository = habitacionRepository;
            _mapper = mapper;
        }

        #region Método Privado

        private bool EsTransicionValida(EstadoReservacion actual, EstadoReservacion nuevo)
        {
            return actual switch
            {
                EstadoReservacion.Pendiente =>
                    nuevo == EstadoReservacion.Confirmada ||
                    nuevo == EstadoReservacion.Cancelada,

                EstadoReservacion.Confirmada =>
                    nuevo == EstadoReservacion.Activa ||
                    nuevo == EstadoReservacion.Cancelada,

                EstadoReservacion.Activa =>
                    nuevo == EstadoReservacion.Finalizada,

                _ => false
            };
        }

        #endregion


        public async Task<bool> CambiarEstadoAsync(int reservacionId, EstadoReservacion nuevoEstado)
        {
            var reservacion = await _repository.ObtenerPorIdAsync(reservacionId);
            if (reservacion == null)
                throw new Exception("La reservación no existe.");

            if(reservacion.Estado == nuevoEstado)
                throw new InvalidOperationException("La reservación ya tiene ese estado.");

            if (!EsTransicionValida(reservacion.Estado, nuevoEstado))
                throw new InvalidOperationException("Transición de estado no permitida.");

            var habitacion = await _habitacionRepository.ObtenerPorIdAsync(reservacion.HabitacionId);
            if (habitacion == null)
                throw new Exception("La habitación no existe.");         

            /*** Cambiar estado de reservación ***/
            reservacion.Estado = nuevoEstado;

            if (nuevoEstado == EstadoReservacion.Confirmada ||
                nuevoEstado == EstadoReservacion.Activa)
            {
                habitacion.EstaDisponible = false;
            }

            if (nuevoEstado == EstadoReservacion.Finalizada ||
                nuevoEstado == EstadoReservacion.Cancelada)
            {
                habitacion.EstaDisponible = true;
            }

            await _repository.GuardarCambiosAsync();
            return true;
        }

        public async Task<ReservacionDto> CambiarFechaSalidaAsync(ReservacionCambiarFechaSalidaDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));

            var reservacion = await _repository.ObtenerPorIdAsync(dto.ReservacionId);
            if(reservacion == null)
                throw new KeyNotFoundException("Reservación no encontrada.");

            if (reservacion.Estado == EstadoReservacion.Finalizada ||reservacion.Estado == EstadoReservacion.Cancelada)
                throw new InvalidOperationException("No se puede modificar una reservación finalizada o cancelada.");

            if (dto.NuevaFechaSalida <= reservacion.FechaEntrada)
                throw new InvalidOperationException("La nueva fecha de salida debe ser mayor que la fecha de entrada.");

            var existeCruce = await _repository.ExisteReservacionActivaEnRangoAsync(reservacion.HabitacionId, reservacion.FechaEntrada, dto.NuevaFechaSalida, reservacion.Id);
            if (existeCruce)
                throw new InvalidOperationException("La nueva fecha genera conflicto con otra reservación.");


            var dias = dto.NuevaFechaSalida.DayNumber - reservacion.FechaEntrada.DayNumber;

            reservacion.FechaSalida = dto.NuevaFechaSalida;
            reservacion.MontoTotal = dias * reservacion.Habitacion.PrecioNoche;

            await _repository.ActualizarAsync(reservacion);

            return _mapper.Map<ReservacionDto>(reservacion);

        }

        public async Task<bool> CancelarAsync(int id)
        {
            var reservacion = await _repository.ObtenerPorIdAsync(id);
            if (reservacion == null)
                throw new KeyNotFoundException("Reservación no encontrada.");

            if (!EsTransicionValida(reservacion.Estado, EstadoReservacion.Cancelada))
                throw new InvalidOperationException("No se puede cancelar esta reservación.");

            var habitacion = await _habitacionRepository.ObtenerPorIdAsync(reservacion.HabitacionId);
            if (habitacion == null)
                throw new Exception("La habitación no existe.");

            // Liberar solo si estaba ocupando habitación
            if (reservacion.Estado == EstadoReservacion.Confirmada ||
                reservacion.Estado == EstadoReservacion.Activa)
            {
                habitacion.EstaDisponible = true;
            }

            reservacion.Estado = EstadoReservacion.Cancelada;            

            await _repository.ActualizarAsync(reservacion);
            return true;
        }

        public async Task<int> ContarTodasAsync()
        {
            return await _repository.ContarTodasAsync();
        }

        public async Task<ReservacionDto> CrearAsync(ReservacionCrearDto dto, string usuarioId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new InvalidOperationException("Usuario inválido.");

            var hoy = DateOnly.FromDateTime(DateTime.Now);
            if (dto.FechaEntrada < hoy)
                throw new InvalidOperationException("la fecha de entrada no puede ser menor que la fecha actual.");

            if (dto.FechaSalida <= dto.FechaEntrada)
                throw new InvalidOperationException("La fecha de salida debe ser mayor que la fecha de entrada.");

            var habitacion = await _habitacionRepository.ObtenerPorIdAsync(dto.HabitacionId);
            if (habitacion == null)
                throw new KeyNotFoundException("Habitación no encontrada.");

            // Validar cruce de fechas
            var existeCruce = await _repository.ExisteReservacionActivaEnRangoAsync(dto.HabitacionId, dto.FechaEntrada, dto.FechaSalida);
            if (existeCruce)
                throw new InvalidOperationException("La habitación ya está reservada en ese rango de fechas.");

            var dias = dto.FechaSalida.DayNumber - dto.FechaEntrada.DayNumber;

            var reservacion = _mapper.Map<Reservacion>(dto);

            reservacion.UsuarioId = usuarioId;
            reservacion.MontoTotal = dias * habitacion.PrecioNoche;
            reservacion.Estado = EstadoReservacion.Pendiente;

            await _repository.CrearAsync(reservacion);

            return _mapper.Map<ReservacionDto>(reservacion);
        }

        public async Task<ReservacionDto> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");

            var reservacion = await _repository.ObtenerPorIdAsync(id);
            if (reservacion == null)
                throw new KeyNotFoundException("Reservación no encontrada.");

            return _mapper.Map<ReservacionDto>(reservacion);
        }

        public async Task<IEnumerable<ReservacionDto>> ObtenerPorUsuarioAsync(string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new InvalidOperationException("Usuario inválido.");

            var reservaciones = await _repository.ObtenerPorUsuarioAsync(usuarioId);
            return _mapper.Map<IEnumerable<ReservacionDto>>(reservaciones);
        }

        public async Task<IEnumerable<ReservacionDto>> ObtenerTodasAsync(int pagina, int tamanoPagina)
        {
            var reservaciones = await _repository.ObtenerTodasAsync(pagina, tamanoPagina);
            return _mapper.Map<IEnumerable<ReservacionDto>>(reservaciones);
        }
    }
}
