using Microsoft.EntityFrameworkCore;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;
using netflix_back.Infrastructure.Data;

namespace netflix_back.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AppDbContext _context;

    public GenreRepository(AppDbContext context)
    {
        _context = context;
    }
    // ------------------------------------------
    
    // Get all list of genres:
    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres.ToListAsync();
    }
}