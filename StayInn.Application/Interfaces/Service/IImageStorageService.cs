
namespace StayInn.Application.Interfaces.Service
{
    public interface IImageStorageService
    {
        Task<string> SubirImagenAsync(Stream fileStream, string filename, string contentType, string folder);

        Task EliminarImagenAsync(string imageUrl);
    }
}
