using netflix_back.Domain.Enums;

namespace netflix_back.Domain.Entities;

public class Content
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int GenresId { get; set; }
    public ContentType ContentType { get; set; }
    public int? VideoId { get; set; }
    public bool IsActive { get; set; }
    
    
    // Relations:
    public Genre Genre { get; set; }
    public Video Video { get; set; }
}