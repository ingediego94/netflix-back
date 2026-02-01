using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;

namespace netflix_back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    
    // ------------------------------------------------------
    
    // Get all Users:
    [Authorize(Roles = "Admin")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any())
                return NotFound(new { message = "Sin usuarios registrados." });

            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener usuarios.", details = ex.Message });
        }
    }
    
    
    // Get by Id User:
    [Authorize(Roles="Admin")]
    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetByIdUser(int id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"El usuario con id '{id}' no fue encontrado." });
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al buscar el usuario.", details = ex.Message });
        }
    }
    
    
    // Update User:
    [Authorize(Roles = "Admin, User")]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedUser = await _userService.UpdateAsync(id, dto);
            if (updatedUser == null)
                return NotFound(new { message = $"Usuario con el id '{id}' no encontrado." });

            return Ok(new { message = "Usuario actualizado correctamente", updatedUser });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar el usuario.", details = ex.Message });
        }
    } 
    
    
    // Delete User:
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var success = await _userService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Usuario con id '{id}' no encontrado." });

            return Ok(new { message = $"Usuario con id '{id}' eliminado correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al eliminar el usuario.", details = ex.Message });
        }
    }
}