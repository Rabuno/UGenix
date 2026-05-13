using UGenix.Shared.Abstractions;

namespace UGenix.Application.Abstractions;

public interface IJwtService
{
    string GenerateAccessToken(UserId userId, string email, IEnumerable<string> permissions);
    string GenerateRefreshToken();
}
