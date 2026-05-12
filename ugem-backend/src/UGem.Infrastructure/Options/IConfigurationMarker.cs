namespace UGem.Infrastructure.Options;

/// <summary>
/// Marker for configuration that is critical for system operation.
/// Failure to load this configuration must prevent application startup.
/// </summary>
public interface ICriticalConfiguration { }

/// <summary>
/// Marker for configuration that is sensitive but can be optional or have 
/// safe defaults in non-production environments.
/// </summary>
public interface ISensitiveConfiguration { }

/// <summary>
/// Marker for non-sensitive configuration that can have defaults.
/// </summary>
public interface INonSensitiveConfiguration { }
