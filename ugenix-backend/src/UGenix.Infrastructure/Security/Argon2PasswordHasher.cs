using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using UGenix.Shared.Abstractions;

namespace UGenix.Infrastructure.Security;

public class Argon2PasswordHasher : IPasswordHasher
{
    // Production parameters (OWASP aligned)
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 4;
    private const int MemorySize = 65536; // 64MB
    private const int DegreeOfParallelism = 1;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize
        };

        var hash = argon2.GetBytes(HashSize);
        
        // Format: v1.Memory.Iterations.Parallelism.Salt.Hash
        return $"v1.{MemorySize}.{Iterations}.{DegreeOfParallelism}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string hash)
    {
        var parts = hash.Split('.');
        if (parts.Length != 6) return false;

        var memorySize = int.Parse(parts[1]);
        var iterations = int.Parse(parts[2]);
        var parallelism = int.Parse(parts[3]);
        var salt = Convert.FromBase64String(parts[4]);
        var storedHash = Convert.FromBase64String(parts[5]);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = parallelism,
            Iterations = iterations,
            MemorySize = memorySize
        };

        var newHash = argon2.GetBytes(HashSize);
        return CryptographicOperations.FixedTimeEquals(newHash, storedHash);
    }
}

