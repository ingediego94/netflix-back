namespace netflix_back.Domain.Entities;

public class Video
{
    public int Id { get; set; }
    public string? UrlPicture {get; set;}
    public string? PublicIdPicture { get; set; } // Nuevo: Para borrar la imagen
    public string UrlVideo { get; set; } = string.Empty;
    public string PublicIdVideo { get; set; } = string.Empty; // Nuevo: Para borrar el video
    public int Duration { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relations 1:1
    public Content Content { get; set; }
}