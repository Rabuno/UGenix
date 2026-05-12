using UGenix.Domain.Abstractions;

namespace UGenix.Domain.Entities;

public enum OrderStatus { Pending, Paid, Cancelled, Refunded }

public class Order : BaseEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public Guid VoucherId { get; private set; }
    public decimal Amount { get; private set; }
    public OrderStatus Status { get; private set; }
    public string RedemptionCode { get; private set; } = string.Empty;

    private Order() { }

    public static Order Create(Guid userId, Guid voucherId, decimal amount)
    {
        return new Order
        {
            UserId = userId,
            VoucherId = voucherId,
            Amount = amount,
            Status = OrderStatus.Pending,
            RedemptionCode = Guid.NewGuid().ToString("N").ToUpper()[..8]
        };
    }

    public void MarkAsPaid() => Status = OrderStatus.Paid;
}

