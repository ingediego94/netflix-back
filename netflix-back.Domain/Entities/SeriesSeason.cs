namespace netflix_back.Domain.Entities;

public class SeriesSeason
{
    public int Id { get; set; }
    public int ContentId { get; set; }
    public int SeasonNumber { get; set; }
    public string SeasonName { get; set; }
    public bool IsActive { get; set; }
    
    // Relations:
    public Content Content { get; set; }
    
    
    // Inverse Relations:
    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
}