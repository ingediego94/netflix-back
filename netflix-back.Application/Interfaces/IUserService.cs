using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto?>> GetAllAsync();
    Task<UserResponseDto?> GetByIdAsync(int id);
    Task<UserResponseDto?> UpdateAsync(int id, UserUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}