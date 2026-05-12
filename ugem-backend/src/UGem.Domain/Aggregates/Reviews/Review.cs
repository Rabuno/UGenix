using UGem.Domain.Abstractions;
using UGem.Shared.Abstractions;

namespace UGem.Domain.Aggregates.Reviews;

public enum ReviewStatus { Pending, Approved, Rejected, Flagged }

public sealed class Review : AggregateRoot<ReviewId>
{
    public Review(
        ReviewId id, 
        UserId userId, 
        RestaurantId restaurantId, 
        string content, 
        int rating,
        bool isVerifiedCheckIn) : base(id)
    {
        UserId = userId;
        RestaurantId = restaurantId;
        Content = content;
        Rating = rating;
        IsVerifiedCheckIn = isVerifiedCheckIn;
        Status = ReviewStatus.Pending;
        
        // Weight is higher for verified check-ins
        Weight = isVerifiedCheckIn ? 1.5m : 1.0m;
    }

    public UserId UserId { get; }
    public RestaurantId RestaurantId { get; }
    public string Content { get; private set; }
    public int Rating { get; }
    public bool IsVerifiedCheckIn { get; }
    public decimal Weight { get; }
    public ReviewStatus Status { get; private set; }

    public void Moderate(ReviewStatus newStatus, string? reason)
    {
        Status = newStatus;
        // Raise Domain Event: ReviewModerated
    }
}
