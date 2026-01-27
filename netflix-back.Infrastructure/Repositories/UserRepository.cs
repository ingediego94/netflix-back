using Microsoft.EntityFrameworkCore;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;
using netflix_back.Infrastructure.Data;

namespace netflix_back.Infrastructure.Repositories;

public class UserRepository : IGeneralRepository<User>
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

// -----------------------------------------------

    // Get All:
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    
    // Get by Id:
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    
    // Create:
    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    
    // Update:
    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    
    // Delete:
    public async Task<bool> DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}