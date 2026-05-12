using UGem.Domain.Abstractions;
using UGem.Shared.Abstractions;

namespace UGem.Domain.Aggregates.Orders;

public sealed class Voucher : AggregateRoot<VoucherId>
{
    public Voucher(VoucherId id, string code, int initialStock, Money price) : base(id)
    {
        Code = code;
        Stock = initialStock;
        Price = price;
        IsActive = true;
    }

    public string Code { get; }
    public int Stock { get; private set; }
    public Money Price { get; }
    public bool IsActive { get; private set; }

    public Result Redeem(UserId userId)
    {
        if (!IsActive) return Result.Failure(Error.Validation("Voucher.Inactive", "Voucher is not active."));
        if (Stock <= 0) return Result.Failure(Error.Conflict("Voucher.OutOfStock", "No more vouchers available."));

        Stock--;

        // Raise Domain Event: VoucherRedeemed
        return Result.Success();
    }
}
