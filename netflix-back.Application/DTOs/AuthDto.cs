using netflix_back.Domain.Enums;

namespace netflix_back.Application.DTOs;


// Register:
public class RegisterDto
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}


// Response for register:
public class UserRegisterResponseDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public Role Role { get; set; }
    public bool IsActive { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


// Login:
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}


// Login response Dto:
public class UserAuthResponseDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    
    public Role Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


// To refresh:
public class RefreshDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}


// Revoke Token:
public class RevokeTokenDto
{
    public string Email { get; set; }
}
