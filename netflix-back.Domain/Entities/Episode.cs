namespace netflix_back.Domain.Entities;

public class Episode
{
    public int Id { get; set; }
    public int EpisodeNumber { get; set; }
    public string EpisodeTitle { get; set; }
    public int SeriesSeasonId { get; set; }
    public string EpisodeDescription { get; set; }
    public int VideoId { get; set; }
    public bool IsActive { get; set; }
    
    
    // Relations:
    public SeriesSeason SeriesSeason { get; set; }
    public Video Video { get; set; }
}