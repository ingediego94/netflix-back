using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

public interface ICloudinaryService
{
    Task<string?> UploadVideoAsync(UploadVideoDto entityDto);
}