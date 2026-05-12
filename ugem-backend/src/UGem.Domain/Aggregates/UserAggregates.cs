using UGem.Domain.Abstractions;
using UGem.Shared.Abstractions;

namespace UGem.Domain.Aggregates;

public sealed class User : AggregateRoot<UserId>
{
    private readonly List<UserRole> _roles = new();
    
    public User(UserId id, Email email, string passwordHash) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        CreatedAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

    public void AddRole(Role role)
    {
        if (_roles.All(r => r.RoleId != role.Id))
            _roles.Add(new UserRole(Id, role.Id));
    }
}

public sealed class Session : AggregateRoot<Guid>
{
    public Session(Guid id, UserId userId, string userAgent, string ipAddress) : base(id)
    {
        UserId = userId;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        CreatedAtUtc = DateTime.UtcNow;
        IsRevoked = false;
    }

    public UserId UserId { get; }
    public string UserAgent { get; }
    public string IpAddress { get; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? LastActiveAtUtc { get; set; }
    public bool IsRevoked { get; private set; }

    public void Revoke() => IsRevoked = true;
}

public record Role(Guid Id, string Name, string[] Permissions);
public record UserRole(UserId UserId, Guid RoleId);
