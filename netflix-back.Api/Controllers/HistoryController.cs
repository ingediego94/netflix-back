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
    
    // -----------------------------------------------------------

    // Get all History:
    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<HistoryResponseDto>>> GetAll()
    {
        try
        {
            var result = await _historyService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el historial.", details = ex.Message });
        }
    }
    
    
    // Get By Id History:
    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<HistoryResponseDto>> GetById(int id)
    {
        try
        {
            var result = await _historyService.GetByIdAsync(id);
            if (result == null) 
                return NotFound(new { message = $"No se encontró registro de historial con ID {id}" });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al buscar el registro de historial.", details = ex.Message });
        }
    }
    
    
    // Endpoint para guardar progreso (Crea o Actualiza)
    [HttpPost("save-progress")]
    public async Task<ActionResult<HistoryResponseDto>> SaveProgress([FromBody] HistoryCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _historyService.CreateOrUpdateAsync(dto);
            return Ok(new { message = "Progreso guardado exitosamente.", data = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al intentar guardar el progreso.", details = ex.Message });
        }
    }
    

}