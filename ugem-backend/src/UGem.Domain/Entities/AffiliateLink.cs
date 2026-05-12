using UGem.Shared.Abstractions;

namespace UGem.Domain.Entities;

public class AffiliateLink : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; } // The influencer/partner
    public string ReferralCode { get; private set; } = string.Empty;
    public int TotalClicks { get; private set; }
    public int TotalConversions { get; private set; }
    public decimal CommissionEarned { get; private set; }

    private AffiliateLink() { }

    public static AffiliateLink Create(Guid userId, string code)
    {
        return new AffiliateLink
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ReferralCode = code,
            TotalClicks = 0,
            TotalConversions = 0,
            CommissionEarned = 0
        };
    }

    public void TrackClick() => TotalClicks++;
    public void TrackConversion(decimal commission) 
    {
        TotalConversions++;
        CommissionEarned += commission;
    }
}
