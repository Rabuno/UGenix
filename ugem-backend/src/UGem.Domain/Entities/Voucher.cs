using UGem.Shared.Abstractions;

namespace UGem.Domain.Entities;

public enum VoucherStatus { Available, SoldOut, Expired }

public class Voucher : BaseEntity
{
    public Guid RestaurantId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public decimal DiscountValue { get; private set; }
    public int TotalStock { get; private set; }
    public int RemainingStock { get; private set; }
    public VoucherStatus Status { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private Voucher() { }

    public static Voucher Create(
        Guid restaurantId, 
        string code, 
        decimal price, 
        decimal discount, 
        int stock, 
        DateTime expiresAt)
    {
        return new Voucher
        {
            Id = Guid.NewGuid(),
            RestaurantId = restaurantId,
            Code = code,
            Price = price,
            DiscountValue = discount,
            TotalStock = stock,
            RemainingStock = stock,
            Status = VoucherStatus.Available,
            ExpiresAt = expiresAt
        };
    }

    public Result Purchase()
    {
        if (RemainingStock <= 0) return Result.Failure(new Error("Voucher.SoldOut", "No stock remaining"));
        if (ExpiresAt < DateTime.UtcNow) return Result.Failure(new Error("Voucher.Expired", "Voucher has expired"));

        RemainingStock--;
        if (RemainingStock == 0) Status = VoucherStatus.SoldOut;
        
        return Result.Success();
    }
}
