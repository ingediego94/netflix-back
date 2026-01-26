using netflix_back.Domain.Enums;

namespace netflix_back.Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public Role Role { get; set; } = Role.User;
    
    public string? RefreshToken { get; set; } 
    public DateTime? RefreshTokenExpire { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    
    // Inverse Relation:
    public ICollection<Video> Photos { get; set; } = new List<Video>();
}