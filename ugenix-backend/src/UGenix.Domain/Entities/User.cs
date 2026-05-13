using UGenix.Domain.Abstractions;
using UGenix.Domain.Primitives;

namespace UGenix.Domain.Entities;

public class User : BaseEntity, IAggregateRoot
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash, string role)
    {
        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = role,
            IsActive = true
        };
    }
}

