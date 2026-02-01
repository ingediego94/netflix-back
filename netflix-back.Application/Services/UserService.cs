using AutoMapper;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Application.Services;


public class UserService : IUserService
{
    private readonly IGeneralRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IGeneralRepository<User> userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    // -------------------------------------------------
    
    // Get All:
    public async Task<IEnumerable<UserResponseDto?>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    
    // Get By Id:
    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserResponseDto>(user);
    }
    
    
    // Update:
    public async Task<UserResponseDto?> UpdateAsync(int id, UserUpdateDto dto)
    {

        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return null;

        _mapper.Map(dto, user);
        
        // SEGURIDAD: Solo actualizar password si se proporciona uno nuevo
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponseDto>(updatedUser);
    }

    // Delete:
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if(user == null) 
            return false;

        var toDelete = await _userRepository.DeleteAsync(user);
        return true;
    }
}