using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;    // IFormFile

namespace netflix_back.Application.DTOs;

public class UploadVideoDto
{
    public IFormFile Video { get; set; }
    public int? UserId { get; set; }    // VALIDAR¡
}

public class VideoUploadFormDto
{
    [Required(ErrorMessage = "El archivo de video es requerido.")]
    public IFormFile Video { get; set; }
    
    // We use the same name 'UserId' than in DTO for the service for consistence.
    public int? UserId { get; set; }        // VALIDAR ¡
}