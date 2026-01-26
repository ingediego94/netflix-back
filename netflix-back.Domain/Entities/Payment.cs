using netflix_back.Domain.Enums;

namespace netflix_back.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int SubscriptionId {get; set;}
    public int PaymentMethdId { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public Status Status { get; set; }
    public string TransactionReference { get; set; }
    
    // Relations:
    public Subscription Subscription { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}