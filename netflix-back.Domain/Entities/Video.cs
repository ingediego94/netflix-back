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
    
    // Relations 1:1
    public Content Content { get; set; }
    public Episode Episode { get; set; }

    
    // Inverse Relation:
    public ICollection<Video> Videos { get; set; } = new List<Video>();
}