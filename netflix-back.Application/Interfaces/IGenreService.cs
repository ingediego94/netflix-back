using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreResponseDto>> GetAllAsync();
}