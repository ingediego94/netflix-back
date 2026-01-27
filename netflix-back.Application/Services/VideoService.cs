using AutoMapper;
using Microsoft.AspNetCore.Http;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Application.Services;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository; 
    private readonly IMapper _mapper;
    private readonly ICloudinaryService _cloudinaryService; // <- Now we inject CloudinaryService

    public VideoService(IVideoRepository videoRepository, 
        IMapper mapper, 
        ICloudinaryService cloudinaryService)
    {
        _videoRepository = videoRepository;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
    }

    // -----------------------------------------------------------------
    
    // Get All Videos:
    public async Task<IEnumerable<VideoResponseDto>?> GetAllVideosAsync()
    {
        var videos = await _videoRepository.GetAllAsync();
        if (videos == null) return null; 

        return _mapper.Map<IEnumerable<VideoResponseDto>>(videos);
    }

    
    // Add Video:
    public async Task<VideoResponseDto?> AddVideoAsync(VideoAddDto videoDto)
    {
        // 1. Subir Video (Obtenemos duración y PublicId automáticamente)
        var videoRes = await _cloudinaryService.UploadAsync(new UploadVideoDto { Video = videoDto.VideoFile });
        if (videoRes == null) throw new Exception("Error al subir video.");

        // 2. Subir Foto opcional
        CloudinaryUploadResult? photoRes = null;
        if (videoDto.PhotoFile != null)
        {
            photoRes = await _cloudinaryService.UploadAsync(new UploadVideoDto { Video = videoDto.PhotoFile });
        }

        // 3. Guardar en DB
        var newVideo = new Video
        {
            UrlVideo = videoRes.Url!,
            PublicIdVideo = videoRes.PublicId!,
            Duration = videoRes.Duration, // MODIFICADO: Sin librerías externas
            UrlPicture = photoRes?.Url,
            PublicIdPicture = photoRes?.PublicId,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var added = await _videoRepository.AddAsync(newVideo);
        return _mapper.Map<VideoResponseDto>(added);
    }

    
    
    // Change video visibility:
    public async Task<bool> ChangeVideoStatusAsync(int id)
    {
        return await _videoRepository.ChangeStatus(id);
    }

    
    
    // Remove video:
    public async Task<bool> RemoveVideoAsync(int id)
    {
        // 1. Obtener datos del video de la DB antes de borrar
        var video = await _videoRepository.GetByIdAsync(id); // Asegúrate de tener este método en tu Repo
        if (video == null) return false;

        // 2. Borrar de Cloudinary
        await _cloudinaryService.DeleteFileAsync(video.PublicIdVideo, "video");
        if (!string.IsNullOrEmpty(video.PublicIdPicture))
        {
            await _cloudinaryService.DeleteFileAsync(video.PublicIdPicture, "image");
        }

        // 3. Borrar de DB
        return await _videoRepository.RemoveAsync(id);
    }
}