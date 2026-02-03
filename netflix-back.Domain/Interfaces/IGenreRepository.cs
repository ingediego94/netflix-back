using netflix_back.Domain.Entities;

namespace netflix_back.Domain.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
}