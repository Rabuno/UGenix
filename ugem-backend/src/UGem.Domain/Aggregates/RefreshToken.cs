using System.Security.Cryptography;
using System.Text;

namespace UGem.Domain.Aggregates;

public sealed class RefreshToken
{
    public Guid Id { get; init; }
    public Guid FamilyId { get; init; } // Tracks the lineage of tokens
    public string TokenHash { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
    public DateTime ExpiresAtUtc { get; init; }
    public bool IsUsed { get; private set; }
    public bool IsRevoked { get; private set; }

    public static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }

    public void MarkAsUsed() => IsUsed = true;
    public void Revoke() => IsRevoked = true;
    
    public bool IsExpired(DateTime now) => now >= ExpiresAtUtc;
    public bool IsValid(DateTime now) => !IsUsed && !IsRevoked && !IsExpired(now);
}
