using AutoMapper;
using StayInn.Application.DTOs.AreaEsparcimiento;
using StayInn.Application.DTOs.Habitacion;
using StayInn.Application.DTOs.Hotel;
using StayInn.Application.DTOs.Reservacion;
using StayInn.Application.DTOs.Usuario;
using StayInn.Domain.Entities;

namespace StayInn.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Hotel (Adim)
            CreateMap<Hotel, HotelDto>();
            CreateMap<HotelCrearDto, Hotel>();
            CreateMap<HotelActualizarDto, Hotel>();


            // Áreas de Esparcimiento
            CreateMap<AreaEsparcimiento, AreaEsparcimientoHomeDto>();

            CreateMap<AreaEsparcimiento, AreaEsparcimientoDto>();
            CreateMap<AreaEsparcimientoCrearDto, AreaEsparcimiento>();
            CreateMap<AreaEsparcimientoActualizarDto, AreaEsparcimiento>();


            // Habitaciones
            CreateMap<Habitacion, HabitacionDto>();
            CreateMap<HabitacionCrearDto, Habitacion>();
            CreateMap<HabitacionActualizarDto, Habitacion>();


            // Reservaciones
            CreateMap<Reservacion, ReservacionResponseDto>()
                .ForMember(dest => dest.NumeroHabitacion,
                    opt => opt.MapFrom(src => src.Habitacion.Numero))
                .ForMember(dest => dest.Estado,
                    opt => opt.MapFrom(src => src.Estado.ToString()));

            CreateMap<ReservacionCrearDto, Reservacion>()
                .ForMember(dest => dest.Estado,
                    opt => opt.Ignore()) // lo maneja el dominio
                .ForMember(dest => dest.MontoTotal,
                    opt => opt.Ignore()); // se calcula en servicio

            CreateMap<ReservacionCambiarFechaSalidaDto, Reservacion>()
                .ForMember(dest => dest.FechaSalida,
                    opt => opt.MapFrom(src => src.NuevaFechaSalida));


            // Usuarios
            CreateMap<ApplicationUser, UsuarioResponseDto>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email!))
                .ForMember(dest => dest.Rol,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Token,
                    opt => opt.Ignore());

            CreateMap<UsuarioRegistroDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email));
        }
    }
}
