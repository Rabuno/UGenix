using UGem.Domain.Abstractions;
using UGem.Shared.Abstractions;

namespace UGem.Domain.Aggregates.CheckIns;

public sealed class CheckIn : AggregateRoot<Guid>
{
    public CheckIn(
        Guid id, 
        UserId userId, 
        RestaurantId restaurantId, 
        DateTime occurredAtUtc) : base(id)
    {
        UserId = userId;
        RestaurantId = restaurantId;
        OccurredAtUtc = occurredAtUtc;
    }

    public UserId UserId { get; }
    public RestaurantId RestaurantId { get; }
    public DateTime OccurredAtUtc { get; }
}

public interface IGeoValidationService
{
    bool IsWithinRestaurantRange(double userLat, double userLon, RestaurantId restaurantId);
    bool DetectSpoofing(UserId userId, double userLat, double userLon);
}
