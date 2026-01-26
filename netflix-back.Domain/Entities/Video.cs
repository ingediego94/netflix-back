namespace netflix_back.Domain.Entities;

public class Video
{
    public int Id { get; set; }
    public string? UrlPicture {get; set;}
    public string UrlVideo { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    
    // Inverse Relations:
    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    public ICollection<Content> Contents { get; set; } = new List<Content>();
    
}