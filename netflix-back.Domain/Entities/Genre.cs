namespace netflix_back.Domain.Entities;

public class Genre
{
    public int Id { get; set; }
    public string GenreName { get; set; }
    public bool IsActive { get; set; }
    
    // Inverse Relation:
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}