using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

public interface IVideoService
{
    Task<IEnumerable<VideoResponseDto>?> GetAllVideosAsync();
    Task<VideoResponseDto?> AddVideoAsync(VideoAddDto videoDto);
    Task<bool> ChangeVideoStatusAsync(int id);
    Task<bool> RemoveVideoAsync(int id);
}