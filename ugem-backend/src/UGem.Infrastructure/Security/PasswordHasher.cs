using BC = BCrypt.Net.BCrypt;

namespace UGem.Infrastructure.Security;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public class PasswordHasher : IPasswordHasher
{
    // Work factor 12 for production security (adjust based on hardware)
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        return BC.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BC.Verify(password, hash);
    }
}
