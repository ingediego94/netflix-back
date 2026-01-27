using AutoMapper;
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
        if (videoDto == null)
            throw new ArgumentNullException(nameof(videoDto), "El DTO del video no puede ser nulo.");
        
        // 1. Upload video to Cloudinary.
        var videoUploadDto = new UploadVideoDto
        {
            Video = videoDto.VideoFile,
            UserId = null
        };
        
        var urlVideo = await _cloudinaryService.UploadVideoAsync(videoUploadDto);

        if (string.IsNullOrEmpty(urlVideo))
        {
            throw new InvalidOperationException("No se pudo subir la imagen a Cloudinary."); 
        }

        // 2. Upload picture (Thumbnail) to Cloudinary if exists.
        string? urlPicture = null;
        if (videoDto.PhotoFile != null)
        {
            var photoUploadDto = new UploadVideoDto
            {
                Video = videoDto.PhotoFile,
                UserId = null
            };
            urlPicture = await _cloudinaryService.UploadVideoAsync(photoUploadDto);
        }
        
        
        // 3. Creating the entity and save on the database according with DTO and Cloudinary
        var newVideo = new Video
        {
            UrlPicture = urlPicture,         // MODIFICADO: Viene de la subida opcional de imagen
            UrlVideo = urlVideo,             // MODIFICADO: Viene de la subida de video
            Duration = videoDto.Duration,    // MODIFICADO: Viene directamente del DTO de entrada
            Active = true, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var addedVideo = await _videoRepository.AddAsync(newVideo);

        if (addedVideo == null) 
        {
            // Optional: If the addition fails after uploading, it is recommended
            // to implement a logic to delete the picture of Cloudinary (rollback).
            return null;
        }

        return _mapper.Map<VideoResponseDto>(addedVideo);
    }

    
    
    // Change video visibility:
    public async Task<bool> ChangeVideoStatusAsync(int id)
    {
        return await _videoRepository.ChangeStatus(id);
    }

    
    
    // Remove video:
    public async Task<bool> RemoveVideoAsync(int id)
    {
        // Logic to delete on DB
        var removed = await _videoRepository.RemoveAsync(id);
        // Logic to delete on Cloudinary (if you want)
        return removed;
    }
}