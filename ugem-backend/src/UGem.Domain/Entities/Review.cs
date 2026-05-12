using UGem.Shared.Abstractions;

namespace UGem.Domain.Entities;

public class Review : Entity<Guid>, IAggregateRoot
{
    public Guid RestaurantId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; } // 1-5
    public string Comment { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    
    // Anti-Fraud Metadata
    public string IpAddress { get; private set; } = string.Empty;
    public string UserAgent { get; private set; } = string.Empty;
    public bool IsVerified { get; private set; } // Verified purchase/check-in

    private Review() { } // EF Core

    public static Review Create(
        Guid restaurantId, 
        Guid userId, 
        int rating, 
        string comment,
        string ipAddress,
        string userAgent,
        bool isVerified)
    {
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5");
        
        return new Review
        {
            Id = Guid.NewGuid(),
            RestaurantId = restaurantId,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsVerified = isVerified
        };
    }
}
