using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGenix.Domain.Entities;
using UGenix.Shared.Abstractions;

namespace UGenix.API.Controllers;

[ApiVersion("1.0")]
public class VoucherController(MediatR.ISender mediator) : BaseApiController(mediator)
{
    /// <summary>
    /// Get available vouchers for a specific restaurant.
    /// </summary>
    [HttpGet("restaurant/{restaurantId}")]
    [AllowAnonymous]
    public IActionResult GetByRestaurant(Guid restaurantId)
    {
        // Mocking retrieval for now
        var vouchers = new List<Voucher>
        {
            Voucher.Create(restaurantId, "UGEM-50", 100000, 50000, 100, DateTime.UtcNow.AddDays(30))
        };
        return Ok(vouchers);
    }

    /// <summary>
    /// Purchase a voucher. Creates a pending order and reserves stock.
    /// </summary>
    [HttpPost("purchase")]
    [Authorize]
    public IActionResult Purchase([FromBody] PurchaseRequest request)
    {
        // 1. Logic would involve MediatR command
        // 2. Load Voucher -> Purchase() -> Create Order -> Save Changes
        var orderId = Guid.NewGuid();
        return Ok(new { OrderId = orderId, Status = "Pending" });
    }
}

public record PurchaseRequest(Guid VoucherId);

