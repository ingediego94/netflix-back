namespace netflix_back.Domain.Entities;

public class Plan
{
    public int Id { get; set; }
    public string PlanName { get; set; }
    public double Price { get; set; }
    public int MaxScreens { get; set; }
    public string VideoQuality { get; set; }
    public bool IsActive { get; set; }
    
    // Inverse Relation:
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}