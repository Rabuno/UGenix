using UGem.Domain.Abstractions;

namespace UGem.Domain.Entities;

public class Review : BaseEntity, IAggregateRoot
{
    public Guid RestaurantId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; } = string.Empty;

    // Anti-Fraud Metadata
    public string UserAgent { get; private set; } = string.Empty;
    public string IpAddress { get; private set; } = string.Empty;

    private Review() { }

    public static Review Create(
        Guid restaurantId, 
        Guid userId, 
        int rating, 
        string comment,
        string userAgent,
        string ipAddress)
    {
        return new Review
        {
            RestaurantId = restaurantId,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            UserAgent = userAgent,
            IpAddress = ipAddress
        };
    }
}
