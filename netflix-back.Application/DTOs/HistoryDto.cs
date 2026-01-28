namespace netflix_back.Application.DTOs;

// Create:
public class HistoryCreateDto
{
    public int UserId { get; set; }
    public int VideoId { get; set; }
    public int Progress {get; set;}
}


// Update:
public class HistoryUpdateDto
{
    public int UserId { get; set; }
    public int VideoId { get; set; }
    public int Progress {get; set;}
}

// History Response:
public class HistoryResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int VideoId { get; set; }
    public int Progress {get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}