using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Application.Services;


public class AuthService : IAuthService
{
    private readonly IGeneralRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public AuthService(IGeneralRepository<User> userRepository,
        IMapper mapper, IConfiguration config)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _config = config;
    }
    
    // Configurable duration:
    private readonly int _jwtMinutes = 40;
    private readonly int _refreshTokenDays = 7;
    
    // --------------------------------------------
    
    // REGISTER:
    public async Task<UserRegisterResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Los datos registrados no pueden ser nulos.");

        if (string.IsNullOrWhiteSpace(registerDto.Email) || string.IsNullOrWhiteSpace(registerDto.Password))
            throw new ArgumentException("El email y la contraseña son campos obligatorios.");

        try
        {
            var users = await _userRepository.GetAllAsync();
            var exist = users.FirstOrDefault(user => user.Email.ToLower() == registerDto.Email.ToLower());

            if (exist != null)
                throw new InvalidOperationException($"El email '{registerDto.Email}' ya se encuentra registrado.");

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.CreateAsync(user);

            return _mapper.Map<UserRegisterResponseDto>(user);
        }
        catch (InvalidOperationException ex)
        {
            // We are capturing logic bussiness errors (as an duplied user)
            throw new Exception(ex.Message);
        }
        catch(Exception ex)
        {
            throw new Exception("Error interno al procesar el registro del usuario. Por favor intente mas tarde.");
        }
    }
    
    

    // LOGIN:
    public async Task<UserAuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var users = await _userRepository.GetAllAsync();
        var exists = users.FirstOrDefault(user => user.Email == loginDto.Email);

        if (exists == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, exists.PasswordHash))
            throw new SecurityException("Incorrect credentials.");
        
        // Making the token + refresh and saves the refresh on DB.
        return await GenerateTokensAsync(exists);
    }


    // REFRESH:
    public async Task<UserAuthResponseDto> RefreshAsync(RefreshDto refreshDto)
    {
        if (refreshDto == null || string.IsNullOrEmpty(refreshDto.Token) ||
            string.IsNullOrEmpty(refreshDto.RefreshToken))
            throw new ArgumentException("Token & RefreshToken are required.");
        
        // 1) Obtain claims even when token has expired.
        var principal = getPrincipalFromExpireToken(refreshDto.Token);

        var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            throw new SecurityException("Invalid Token");

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new SecurityException("User not founded");
        
        // 2) Verify that refresh token it will be the same and has not expired.
        if (user.RefreshToken != refreshDto.RefreshToken || user.RefreshTokenExpire <= DateTime.UtcNow)
            throw new SecurityException("The refreshToken is invalid or has expired. ");
        
        // 3) To generate new tokens and save the new refresh token.
        return await GenerateTokensAsync(user);

    }

    
    // REVOKE:
    public async Task<bool> RevokeAsync(RevokeTokenDto revokeTokenDto)
    {
        if(revokeTokenDto == null || string.IsNullOrEmpty(revokeTokenDto.Email))
            throw new ArgumentException("Email is required.");

        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == revokeTokenDto.Email);
        if (user == null) return false;
        
        
        // Invalidate refresh token
        user.RefreshToken = null;
        user.RefreshTokenExpire = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return true;
    }
    
    
    // GENERATE TOKEM: Json Web Token
    private JwtSecurityToken GenerateToken(User user)
    {
        var secretKey = _config["Jwt:Key"];
        if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
            throw new Exception("La clave JWT debe tener al menos 32 caracteres.");
        
        // 1. Key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        // 2. Algorithm
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // 3. Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        // We create the Jwt
        return new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtMinutes),
            signingCredentials:credentials
        );
    }
    
    
    // REFRESH TOKEN:
    private string GenerateRefresh()
    {
        var array = new byte[32];
        using var rgn = RandomNumberGenerator.Create();
        rgn.GetBytes(array);
        return Convert.ToBase64String(array);
    }
    
    
    // EXPIRE TOKEN:
    private ClaimsPrincipal getPrincipalFromExpireToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            ValidateLifetime = false
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,  StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityException("Token not valid.");

        return principal;
    }
    
    
    
    // GENERATE TOKEN:
    private async Task<UserAuthResponseDto> GenerateTokensAsync(User user)
    {
        var jwtToken = GenerateToken(user);
        var refresh =  GenerateRefresh();

        user.RefreshToken = refresh;
        user.RefreshTokenExpire = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user);

        var response = _mapper.Map<UserAuthResponseDto>(user);

        var handelr = new JwtSecurityTokenHandler();
        response.Token = handelr.WriteToken(jwtToken);
        
        return response;
    }

}