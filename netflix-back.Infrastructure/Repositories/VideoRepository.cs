using Microsoft.EntityFrameworkCore;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;
using netflix_back.Infrastructure.Data;

namespace netflix_back.Infrastructure.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly AppDbContext _context;

    public VideoRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // ---------------------------------------------------
    
    // Get All:
    public async Task<IEnumerable<Video>?> GetAllAsync()
    {
        return await _context.Videos.ToListAsync();
    }

    
    // Add Videos:
    public async Task<Video?> AddAsync(Video video)
    {
        _context.Videos.Add(video);
        await _context.SaveChangesAsync();
        return video;
    }

    
    // Delete:
    public async Task<bool> RemoveAsync(int id)
    {
        var video = await _context.Videos.FirstOrDefaultAsync( v => v.Id == id);
        if (video == null) return false;
        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();
        return true;
    }

    
    // Change status (visible or not):
    public async Task<bool> ChangeStatus(int id)
    {
        var video = await _context.Videos.FirstOrDefaultAsync(v => v.Id == id);
        if (video == null) return false;
        video.Active = !video.Active;
        _context.Videos.Update(video);
        await _context.SaveChangesAsync();
        return true;
    }
}