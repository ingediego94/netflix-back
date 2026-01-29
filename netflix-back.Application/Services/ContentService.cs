using AutoMapper;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Enums;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Application.Services;

public class ContentService : IContentService
{
    private readonly IGeneralRepository<Content> _contentRepo;
    private readonly IVideoRepository _videoRepo;
    private readonly ICloudinaryService _cloudinary;
    private readonly IMapper _mapper;

    public ContentService(
        IGeneralRepository<Content> contentRepo,
        IVideoRepository videoRepo,
        ICloudinaryService cloudinary,
        IMapper mapper)
    {
        _contentRepo = contentRepo;
        _videoRepo = videoRepo;
        _cloudinary = cloudinary;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContentResponseDto>> GetAllAsync()
    {
        var contents = await _contentRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<ContentResponseDto>>(contents);
    }

    public async Task<ContentResponseDto?> GetByIdAsync(int id)
    {
        var content = await _contentRepo.GetByIdAsync(id);
        return _mapper.Map<ContentResponseDto>(content);
    }

    public async Task<ContentResponseDto> CreateAsync(ContentCreateDto dto)
    {
        // 1. Subir Video y Foto a Cloudinary
        var videoRes = await _cloudinary.UploadAsync(new UploadVideoDto { Video = dto.VideoFile });
        if (videoRes == null) throw new Exception("Error al subir video.");

        CloudinaryUploadResult? photoRes = null;
        if (dto.PhotoFile != null)
            photoRes = await _cloudinary.UploadAsync(new UploadVideoDto { Video = dto.PhotoFile });

        // 2. Crear registro Video
        var video = new Video
        {
            UrlVideo = videoRes.Url!,
            PublicIdVideo = videoRes.PublicId!,
            Duration = videoRes.Duration,
            UrlPicture = photoRes?.Url,
            PublicIdPicture = photoRes?.PublicId,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var addedVideo = await _videoRepo.AddAsync(video);

        // 3. Crear registro Content vinculado
        var content = new Content
        {
            Title = dto.Title,
            GenreId = dto.GenresId,
            ContentType = ContentType.Movie,
            VideoId = addedVideo!.Id,
            IsActive = true
        };
        var addedContent = await _contentRepo.CreateAsync(content);

        return _mapper.Map<ContentResponseDto>(addedContent);
    }

    public async Task<ContentResponseDto?> UpdateAsync(int id, ContentUpdateDto dto)
    {
        var content = await _contentRepo.GetByIdAsync(id);
        if (content == null) return null;

        var video = await _videoRepo.GetByIdAsync(content.VideoId!.Value);

        // Actualizar datos de Content
        if (!string.IsNullOrEmpty(dto.Title)) content.Title = dto.Title;
        if (dto.GenresId.HasValue) content.GenreId = dto.GenresId.Value;

        // Actualizar archivos si vienen nuevos
        if (dto.VideoFile != null)
        {
            await _cloudinary.DeleteFileAsync(video!.PublicIdVideo, "video");
            var res = await _cloudinary.UploadAsync(new UploadVideoDto { Video = dto.VideoFile });
            video.UrlVideo = res!.Url!;
            video.PublicIdVideo = res.PublicId!;
            video.Duration = res.Duration;
        }

        if (dto.PhotoFile != null)
        {
            if (!string.IsNullOrEmpty(video!.PublicIdPicture))
                await _cloudinary.DeleteFileAsync(video.PublicIdPicture, "image");
            
            var res = await _cloudinary.UploadAsync(new UploadVideoDto { Video = dto.PhotoFile });
            video.UrlPicture = res!.Url;
            video.PublicIdPicture = res.PublicId;
        }

        video.UpdatedAt = DateTime.UtcNow;
        await _contentRepo.UpdateAsync(content);
        return _mapper.Map<ContentResponseDto>(content);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var content = await _contentRepo.GetByIdAsync(id);
        if (content == null) return false;

        var video = await _videoRepo.GetByIdAsync(content.VideoId!.Value);
        if (video != null)
        {
            await _cloudinary.DeleteFileAsync(video.PublicIdVideo, "video");
            if (!string.IsNullOrEmpty(video.PublicIdPicture))
                await _cloudinary.DeleteFileAsync(video.PublicIdPicture, "image");
            
            await _videoRepo.RemoveAsync(video.Id);
        }

        return await _contentRepo.DeleteAsync(content);
    }
}