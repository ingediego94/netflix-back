using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

// Objeto para capturar URL, PublicId (necesario para borrar) y Duración
public class CloudinaryUploadResult
{
    public string? Url { get; set; }
    public string? PublicId { get; set; } // Nuevo: Para poder borrar el archivo después
    public int Duration { get; set; }
}

public interface ICloudinaryService
{
    Task<CloudinaryUploadResult?> UploadAsync(UploadVideoDto entityDto);
    Task<bool> DeleteFileAsync(string publicId, string resourceType); // Nuevo: "video" o "image"
}