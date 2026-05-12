using UGem.Domain.Abstractions;

namespace UGem.Domain.Entities;

public class AffiliateLink : BaseEntity, IAggregateRoot
{
    public Guid RestaurantId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string TargetUrl { get; private set; } = string.Empty;
    public int ClickCount { get; private set; }

    private AffiliateLink() { }

    public static AffiliateLink Create(Guid restaurantId, string code, string targetUrl)
    {
        return new AffiliateLink
        {
            RestaurantId = restaurantId,
            Code = code,
            TargetUrl = targetUrl,
            ClickCount = 0
        };
    }

    public void RecordClick() => ClickCount++;
}
