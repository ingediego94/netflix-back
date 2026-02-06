using AutoMapper;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;

    public GenreService(IGenreRepository genreRepository, IMapper mapper)
    {
        _genreRepository = genreRepository;
        _mapper = mapper;
    }
    
    // -------------------------------------------------
    
    // Get All:
    public async Task<IEnumerable<GenreResponseDto>> GetAllAsync()
    {
        var results = await _genreRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<GenreResponseDto>>(results);
    }
}