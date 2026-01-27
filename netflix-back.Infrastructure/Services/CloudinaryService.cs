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
        var cloudName =
            (Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME")
            ?? configuration["Cloudinary:CloudName"])?.Trim();

        var apiKey =
            (Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")
            ?? configuration["Cloudinary:ApiKey"])?.Trim();

        var apiSecret =
            (Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
            ?? configuration["Cloudinary:ApiSecret"])?.Trim();

        if (string.IsNullOrWhiteSpace(cloudName) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiSecret))
        {
            throw new InvalidOperationException("Cloudinary credentials are missing");
        }

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account)
        {
            Api = { Secure = true }
        };
    }

    public async Task<string?> UploadVideoAsync(UploadVideoDto dto)
    {
        if (dto == null || dto.Video == null)
            throw new ArgumentNullException(nameof(dto));

        var contentType = dto.Video.ContentType; // ---> MODIFICADO (nuevo)

        // ---------------- IMAGES ----------------
        if (contentType.StartsWith("image/")) // ---> MODIFICADO (nuevo)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(
                    dto.Video.FileName,
                    dto.Video.OpenReadStream()
                ),
                Folder = dto.UserId.HasValue
                    ? $"netflix/user/images/{dto.UserId.Value}"
                    : "netflix/images/general"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK &&
                result.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception(result.Error?.Message ?? "Cloudinary image upload failed");
            }

            return result.SecureUrl?.AbsoluteUri;
        }

        // ---------------- VIDEOS ----------------
        if (contentType.StartsWith("video/")) // ---> MODIFICADO (nuevo)
        {
            var uploadParams = new VideoUploadParams // ---> MODIFICADO (nuevo)
            {
                File = new FileDescription(
                    dto.Video.FileName,
                    dto.Video.OpenReadStream()
                ),
                Folder = dto.UserId.HasValue
                    ? $"netflix/user/videos/{dto.UserId.Value}" // ---> MODIFICADO (nuevo)
                    : "netflix/videos/general"                  // ---> MODIFICADO (nuevo)
            };

            var result = await _cloudinary.UploadAsync(uploadParams); // ---> MODIFICADO

            if (result.StatusCode != System.Net.HttpStatusCode.OK &&
                result.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception(result.Error?.Message ?? "Cloudinary video upload failed");
            }

            return result.SecureUrl?.AbsoluteUri;
        }

        throw new InvalidOperationException("Unsupported file type"); // ---> MODIFICADO (nuevo)
    }
}