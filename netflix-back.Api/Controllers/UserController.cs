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
        var users = await _userService.GetAllAsync();

        if (users == null)
            return NotFound(
                new { message = "Sin usuarios registrados."}
            );

        return Ok(users);
    }
    
    
    // Get by Id User:
    [Authorize(Roles="Admin")]
    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetByIdUser(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound(
                new { message = $"El usuario con id '{id}' no fue encontrado. " }
            );
        
        return Ok(user);
    }
    
    
    // Update User:
    [Authorize(Roles = "Admin, User")]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedUser = await _userService.UpdateAsync(id, dto);

        if (updatedUser == null)
            return NotFound(
                new { message = $"Usuario con el id '{id}' no encontrado." }
            );

        return Ok(
            new { message = $"Usuario con el id '{id}' actualizado correctamente", updatedUser }
        );
    } 
    
    
    // Delete User:
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var toDelete = await _userService.DeleteAsync(id);

        if (!toDelete)
            return NotFound(
                new { message = $"Usuario con id '{id}' no encontrado." }
            );

        return Ok(
            new {message = $"Usuario con id '{id}' eliminado correctamente."}
            );
    }
}