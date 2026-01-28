using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;

namespace netflix_back.Api.Controllers;

// [Authorize] 
[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;

    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<HistoryResponseDto>>> GetAll()
    {
        var result = await _historyService.GetAllAsync();
        return Ok(result);
    }
    
    
    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<HistoryResponseDto>> GetById(int id)
    {
        var result = await _historyService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }
    
    
    // Endpoint para guardar progreso (Crea o Actualiza)
    [HttpPost("save-progress")]
    public async Task<ActionResult<HistoryResponseDto>> SaveProgress([FromBody] HistoryCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _historyService.CreateOrUpdateAsync(dto);
        return Ok(result);
    }
    

}