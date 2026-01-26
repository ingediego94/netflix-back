namespace netflix_back.Domain.Entities;

public class History
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int VideoId { get; set; }
    public int Progress {get; set;}
    public DateTime CreatedAt { get; set; }
    
    // Relations:
    public User User { get; set; }
    public Video Video { get; set; }
}