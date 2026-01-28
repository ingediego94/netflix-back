using netflix_back.Domain.Entities;

namespace netflix_back.Domain.Interfaces;

public interface IHistoryRepository
{
    Task<IEnumerable<History>> GetAllAsync();
    Task<History?> GetByIdAsync(int id);
    Task<History?> GetByUserAndVideoAsync(int userId, int videoId);
    Task<History> CreateAsync(History history);
    Task<History> UpdateAsync(History history);
}