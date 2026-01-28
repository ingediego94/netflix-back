using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;

namespace netflix_back.Infrastructure.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
    }

    public async Task<CloudinaryUploadResult?> UploadAsync(UploadVideoDto dto)
    {
        if (dto?.Video == null) return null;

        var contentType = dto.Video.ContentType;
        var fileName = dto.Video.FileName;
        var stream = dto.Video.OpenReadStream();

        if (contentType.StartsWith("video/"))
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = "netflix/videos"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            
            // MODIFICADO: Retornamos la duración que Cloudinary ya calculó
            return new CloudinaryUploadResult
            {
                Url = result.SecureUrl?.AbsoluteUri,
                PublicId = result.PublicId,
                Duration = (int)Math.Round(result.Duration) // Cloudinary devuelve segundos en double
            };
        }
        
        if (contentType.StartsWith("image/"))
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = "netflix/images"
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            return new CloudinaryUploadResult 
            { 
                Url = result.SecureUrl?.AbsoluteUri, 
                PublicId = result.PublicId, 
                Duration = 0 
            };
        }

        return null;
    }

    // NUEVO: Método para eliminar archivos de Cloudinary
    public async Task<bool> DeleteFileAsync(string publicId, string resourceType)
    {
        if (string.IsNullOrEmpty(publicId)) return false;

        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = resourceType == "video" ? ResourceType.Video : ResourceType.Image
        };

        var result = await _cloudinary.DestroyAsync(deletionParams);
        return result.Result == "ok";
    }
}