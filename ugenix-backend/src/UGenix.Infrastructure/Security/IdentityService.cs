using UGenix.Application.Abstractions;
using UGenix.Domain.Entities;
using UGenix.Shared.Abstractions;
using UGenix.Shared.Constants;
using UGenix.Domain.Primitives;

namespace UGenix.Infrastructure.Security;

public class IdentityService(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtService jwtService) : IIdentityService
{
    public async Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user is null || !passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return Result<AuthResponse>.Failure(new Error("Identity.InvalidCredentials", "Invalid email or password.", ErrorType.Unauthorized));
        }

        var token = jwtService.GenerateAccessToken(new UserId(user.Id), user.Email, new[] { user.Role });
        
        return Result<AuthResponse>.Success(new AuthResponse(
            token,
            new UserDto(user.Id, user.Email, user.Role)));
    }

    public async Task<Result<AuthResponse>> RegisterAsync(string email, string password, CancellationToken ct = default)
    {
        if (await userRepository.AnyAsync(u => u.Email == email, ct))
        {
            return Result<AuthResponse>.Failure(new Error("Identity.EmailAlreadyExists", "Email is already in use.", ErrorType.Conflict));
        }

        var user = User.Create(
            email,
            passwordHasher.HashPassword(password),
            Roles.Customer);

        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var token = jwtService.GenerateAccessToken(new UserId(user.Id), user.Email, new[] { user.Role });

        return Result<AuthResponse>.Success(new AuthResponse(
            token,
            new UserDto(user.Id, user.Email, user.Role)));
    }
}
