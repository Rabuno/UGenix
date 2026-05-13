using UGenix.Shared.Abstractions;

namespace UGenix.Application.Abstractions;

public record AuthResponse(string AccessToken, UserDto User);
public record UserDto(Guid Id, string Email, string Role);

public interface IIdentityService
{
    Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken ct = default);
    Task<Result<AuthResponse>> RegisterAsync(string email, string password, CancellationToken ct = default);
}
