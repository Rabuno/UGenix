using FluentAssertions;
using UGenix.Domain.Entities;
using Xunit;

namespace UGenix.Application.UnitTests.Domain;

public class VoucherTests
{
    [Fact]
    public void Purchase_ShouldSucceed_WhenStockIsAvailable()
    {
        // Arrange
        var voucher = Voucher.Create(Guid.NewGuid(), "TEST-50", 100, 50, 10, DateTime.UtcNow.AddDays(1));

        // Act
        var result = voucher.Purchase();

        // Assert
        result.IsSuccess.Should().BeTrue();
        voucher.RemainingStock.Should().Be(9);
    }

    [Fact]
    public void Purchase_ShouldFail_WhenStockIsZero()
    {
        // Arrange
        var voucher = Voucher.Create(Guid.NewGuid(), "TEST-0", 100, 50, 0, DateTime.UtcNow.AddDays(1));

        // Act
        var result = voucher.Purchase();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Voucher.SoldOut");
    }

    [Fact]
    public void Purchase_ShouldMarkAsSoldOut_WhenLastStockIsPurchased()
    {
        // Arrange
        var voucher = Voucher.Create(Guid.NewGuid(), "TEST-LAST", 100, 50, 1, DateTime.UtcNow.AddDays(1));

        // Act
        voucher.Purchase();

        // Assert
        voucher.RemainingStock.Should().Be(0);
        voucher.Status.Should().Be(VoucherStatus.SoldOut);
    }
}
