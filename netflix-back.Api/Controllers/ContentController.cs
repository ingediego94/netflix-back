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
    public async Task<IActionResult> GetAll() => Ok(await _contentService.GetAllAsync());

    
    // Gey By Id Content:
    [Authorize(Roles = "Admin, User")]
    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _contentService.GetByIdAsync(id);
        return result != null ? Ok(result) : NotFound();
    }

    
    // Add Content:
    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
    public async Task<IActionResult> Create([FromForm] ContentCreateDto dto)
    {
        var result = await _contentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    
    // Update Content:
    [Authorize(Roles = "Admin")]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromForm] ContentUpdateDto dto)
    {
        var result = await _contentService.UpdateAsync(id, dto);
        return result != null ? Ok(result) : NotFound();
    }

    
    // Delete Content:
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _contentService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}