using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace netflix_back.Application.DTOs;

public class VideoResponseDto
{
    public int Id { get; set; }
    public string? UrlPicture {get; set;}
    public string UrlVideo { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class VideoAddDto
{
    [Required(ErrorMessage = "El archivo de video es requerido.")]
    public IFormFile VideoFile { get; set; }
    public IFormFile? PhotoFile { get; set; }
    
    [Required(ErrorMessage = "La duración es requerida.")]
    public int Duration { get; set; }
    // public int? UserId { get; set; }        // VALIDAR ¡
}
