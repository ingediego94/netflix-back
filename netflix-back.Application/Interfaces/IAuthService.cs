using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

public interface IAuthService
{
    Task<UserRegisterResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<UserAuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<UserAuthResponseDto> RefreshAsync(RefreshDto refreshDto);
    Task<bool> RevokeAsync(RevokeTokenDto revokeTokenDto);
}