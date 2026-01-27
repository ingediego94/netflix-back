using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;

namespace netflix_back.Api.Controllers;

[ApiController]
[Route("api/videos")]
public class VideosController : ControllerBase
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    // [Authorize(Roles = "Admin, User")]
    [HttpGet("getAll")]
    public async Task<ActionResult<List<VideoResponseDto>>> GetAll()
    {
        var videos = await _videoService.GetAllVideosAsync();
        return Ok(videos ?? new List<VideoResponseDto>());
    }

    
    // [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] VideoAddDto videoDto)
    {
        if (!ModelState.IsValid || videoDto.VideoFile == null) return BadRequest(ModelState);

        try
        {
            var result = await _videoService.AddVideoAsync(videoDto);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    
    // [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _videoService.RemoveVideoAsync(id);
        if (!success) return NotFound(new { message = "Video no encontrado." });
        
        return NoContent(); // 204: Borrado exitoso
    }
}