using Microsoft.EntityFrameworkCore;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;
using netflix_back.Infrastructure.Data;

namespace netflix_back.Infrastructure.Repositories;

public class HistoryRepository : IHistoryRepository
{
    private readonly AppDbContext _context;

    public HistoryRepository(AppDbContext context)
    {
        _context = context;
    }
    //-----------------------------------------------------
    
    // Get All:
    public async Task<IEnumerable<History>> GetAllAsync()
    {
        return await _context.Histories
            .Include(h => h.Video)
            .ToListAsync();
    }

    
    // Get By Id:
    public async Task<History?> GetByIdAsync(int id)
    {
        return await _context.Histories
            .Include(h => h.Video)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    
    // Get By User and Video:
    public async Task<History?> GetByUserAndVideoAsync(int userId, int videoId)
    {
        return await _context.Histories
            .FirstOrDefaultAsync(h => h.UserId == userId && h.VideoId == videoId);
    }


    // Create:
    public async Task<History> CreateAsync(History history)
    {
        _context.Histories.Add(history);
        await _context.SaveChangesAsync();
        return history;
    }

    
    // Update:
    public async Task<History> UpdateAsync(History history)
    {
        _context.Histories.Update(history);
        await _context.SaveChangesAsync();
        return history;
    }
}