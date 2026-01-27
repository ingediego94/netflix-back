using netflix_back.Domain.Entities;

namespace netflix_back.Domain.Interfaces;

public interface IVideoRepository
{
    Task<IEnumerable<Video>?> GetAllAsync();
    Task<Video?> AddAsync(Video video);
    Task<bool> RemoveAsync(int id);
    Task<bool> ChangeStatus(int id);
}