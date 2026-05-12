using UGem.Shared.Abstractions;

namespace UGem.Domain.Entities;

public enum OrderStatus { Pending, Paid, Completed, Cancelled }

public class Order : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public Guid VoucherId { get; private set; }
    public decimal Amount { get; private set; }
    public string RedemptionCode { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { }

    public static Order Create(Guid userId, Guid voucherId, decimal amount)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            VoucherId = voucherId,
            Amount = amount,
            RedemptionCode = GenerateRedemptionCode(),
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsPaid() => Status = OrderStatus.Paid;
    
    private static string GenerateRedemptionCode() 
        => new Random().Next(100000, 999999).ToString(); // Simple 6-digit code
}
