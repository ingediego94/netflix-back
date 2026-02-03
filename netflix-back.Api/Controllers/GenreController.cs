using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netflix_back.Application.Interfaces;

namespace netflix_back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    
    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }
    
    // ------------------------------------------------
    
    // Get all genres
    [Authorize(Roles = "Admin, User")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var results = await _genreService.GetAllAsync();
            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los géneros.", details = ex.Message });
        }
    }
}