using Microsoft.AspNetCore.Http;
using netflix_back.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace netflix_back.Application.DTOs;

public class ContentResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int GenresId { get; set; }
    public string GenreName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public VideoResponseDto Video { get; set; } = null!;
}

public class ContentCreateDto
{
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public int GenresId { get; set; }
    [Required] public IFormFile VideoFile { get; set; } = null!;
    public IFormFile? PhotoFile { get; set; }
}

public class ContentUpdateDto
{
    public string? Title { get; set; }
    public int? GenresId { get; set; }
    public IFormFile? VideoFile { get; set; }
    public IFormFile? PhotoFile { get; set; }
}