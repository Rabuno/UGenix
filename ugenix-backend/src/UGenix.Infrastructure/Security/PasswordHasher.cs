using BCrypt.Net;
using UGenix.Shared.Abstractions;

namespace UGenix.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    // Work factor 12 for production security (adjust based on hardware)
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

