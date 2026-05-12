using System.ComponentModel.DataAnnotations;

namespace UGem.Infrastructure.Options;

public class PostgresOptions : ICriticalConfiguration
{
    [Required]
    public string Url { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Operational Recovery: Startup Fail
}

public class SupabaseOptions : ICriticalConfiguration
{
    [Required]
    public string Url { get; set; } = string.Empty;
    
    [Required]
    public string AnonKey { get; set; } = string.Empty;
    
    [Required]
    public string ServiceRoleKey { get; set; } = string.Empty;
    
    [Required]
    public string JwtSecret { get; set; } = string.Empty;
}

public class JwtOptions : ICriticalConfiguration
{
    [Required]
    public string Secret { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, 10080)]
    public int AccessTokenExpirationMinutes { get; set; } = 60;

    [Range(1, 43200)]
    public int RefreshTokenExpirationDays { get; set; } = 7;
}

public class CloudinaryOptions : ISensitiveConfiguration
{
    [Required]
    public string CloudName { get; set; } = string.Empty;
    
    [Required]
    public string ApiKey { get; set; } = string.Empty;
    
    [Required]
    public string ApiSecret { get; set; } = string.Empty;
}

public class MailOptions : ISensitiveConfiguration
{
    [Required, EmailAddress]
    public string Mail { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public string Host { get; set; } = string.Empty;
    
    [Range(1, 65535)]
    public int Port { get; set; }
    
    public string DisplayName { get; set; } = string.Empty;
}

public class GoogleAuthOptions
{
    [Required]
    public string ClientId { get; set; } = string.Empty;
}

public class RedisOptions
{
    [Required]
    public string ConnectionString { get; set; } = "localhost:6379";
}

public class ObservabilityOptions
{
    public string OtlpEndpoint { get; set; } = string.Empty;
    public string SeqUrl { get; set; } = string.Empty;
    public string ServiceName { get; set; } = "UGem.API";
}
