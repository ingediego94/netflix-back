using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;

namespace netflix_back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentsController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentsController(IContentService contentService)
    {
        _contentService = contentService;
    }

    // ---------------------------------------------------
    
    // Get all Contents:
    [Authorize(Roles = "Admin, User")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _contentService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el catálogo.", details = ex.Message });
        }
    } 
        

    
    // Gey By Id Content:
    [Authorize(Roles = "Admin, User")]
    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _contentService.GetByIdAsync(id);
            return result != null ? Ok(result) : NotFound(new { message = $"Contenido con ID {id} no encontrado." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al buscar el contenido.", details = ex.Message });
        }
    }

    
    // Add Content:
    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
    public async Task<IActionResult> Create([FromForm] ContentCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _contentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear el contenido.", details = ex.Message });
        }
    }

    
    // Update Content:
    [Authorize(Roles = "Admin")]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromForm] ContentUpdateDto dto)
    {
        try
        {
            var result = await _contentService.UpdateAsync(id, dto);
            return result != null ? Ok(result) : NotFound(new { message = "No se pudo actualizar: Contenido no encontrado." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar el contenido.", details = ex.Message });
        }
    }

    
    // Delete Content:
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _contentService.DeleteAsync(id);
            return success ? NoContent() : NotFound(new { message = "No se pudo eliminar: Contenido no encontrado." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error crítico al eliminar el contenido.", details = ex.Message });
        }
    }
}