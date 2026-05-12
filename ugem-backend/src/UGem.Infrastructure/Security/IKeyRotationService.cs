using Microsoft.IdentityModel.Tokens;

namespace UGem.Infrastructure.Security;

public interface IKeyRotationService
{
    SecurityKey GetCurrentSigningKey(out string kid);
    IEnumerable<(string kid, SecurityKey key)> GetValidationKeys();
}

// The JwtService now uses IKeyRotationService to fetch keys
// and injects the 'kid' (Key ID) into the JWT header.
// This allows the API to validate tokens even during a key rollover.
