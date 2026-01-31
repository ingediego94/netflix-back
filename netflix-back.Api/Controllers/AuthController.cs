using Microsoft.AspNetCore.Mvc;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;
using netflix_back.Application.Services;

namespace netflix_back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    // -----------------------------------------------------
    
    // LOGIN:
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Unauthorized("Credenciales incorrectas.");

        return Ok(result);          // Debe devolver UserAuthResponseDto

    }
    
    // REGISTER:
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        
        if (request == null)
            return BadRequest("Los datos de registro son requeridos.");
        try
        {
            var result = await _authService.RegisterAsync(request);

            if (result == null)
                return BadRequest(new {message = "No se ha podido registrar el usuario."});

            return Ok(result);          // Debe devolver UserAuthResponseDto
        }
        catch (InvalidOperationException ex)
        {
            // Capturamos el error de "Email duplicado" u otros de lógica de negocio
            // Esto devolverá un Status 400 en lugar de un 500
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error inesperado en el servidor.", details = ex.Message });
        }

    }
    
    
    // REFRESH:
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshDto request)
    {
        var result = await _authService.RefreshAsync(request);

        if (result == null)
            return Unauthorized("Refresh Token inválido.");

        return Ok(result);          // Debe devolver UserAuthResponseDto
    }
    
    
    // REVOKE:
    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenDto request)
    {
        var result = await _authService.RevokeAsync(request);

        if (!result)
            return BadRequest("No se ha revocado el Token");

        return Ok("Token revocado exitosamente.");
    }
    
}           