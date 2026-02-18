using Microsoft.AspNetCore.Http;
using StayInn.Application.DTOs.Hotel;

namespace StayInn.Application.Interfaces.Service
{
    // Clase auxiliar para retornar URLs de imagen
    public class GuardarArchivoResult
    {
        public string UrlImagen { get; set; } = string.Empty;
    }

    public interface IHotelService
    {
        Task<HotelDto?> ObtenerAsync();


        Task<HotelDto> CrearAsync(HotelCrearDto dto, string imageUrl);
        Task<HotelDto> ActualizarAsync(HotelActualizarDto dto, string? nuevaImagenUrl);
    }
}
