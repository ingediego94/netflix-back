using netflix_back.Domain.Enums;

namespace netflix_back.Domain.Entities;

public class PaymentMethod
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public TypePayment TypePayment { get; set; }
    public CardProvider CardProvider { get; set; }
    public string Last4Digits { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
}