using netflix_back.Domain.Enums;

namespace netflix_back.Application.DTOs;

// Create:
public class UserCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
}


// Update
public class UserUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
}


// Response
public class UserResponseDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Role Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}