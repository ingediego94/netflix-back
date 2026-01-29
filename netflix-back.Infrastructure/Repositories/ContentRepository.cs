using Microsoft.EntityFrameworkCore;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;
using netflix_back.Infrastructure.Data;

namespace netflix_back.Infrastructure.Repositories;

public class ContentRepository : IGeneralRepository<Content>
{
    private readonly AppDbContext _context;

    public ContentRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // --------------------------------------------------
    
    // Get All Contents:
    public async Task<IEnumerable<Content>> GetAllAsync()
    {
        return await _context.Contents.ToListAsync();
    }

    // Get Content By Id:
    public async Task<Content?> GetByIdAsync(int id)
    {
        return await _context.Contents.FindAsync(id);
    }
    

    // Create Content:
    public async Task<Content> CreateAsync(Content content)
    {
        _context.Contents.Add(content);
        await _context.SaveChangesAsync();
        return content;
    }

    
    // Update Content:
    public async Task<Content> UpdateAsync(Content content)
    {
        _context.Contents.Update(content);
        await _context.SaveChangesAsync();
        return content;
    }

    
    // Delete Content:
    public async Task<bool> DeleteAsync(Content content)
    {
        _context.Contents.Remove(content);
        await _context.SaveChangesAsync();
        return true;
    }
}