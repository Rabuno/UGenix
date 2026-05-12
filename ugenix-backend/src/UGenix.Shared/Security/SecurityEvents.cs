namespace UGenix.Shared.Security;

public enum SecurityEventSeverity
{
    Informational = 0,
    Warning = 1,
    Critical = 2
}

public enum SecurityEventType
{
    LoginSuccess = 0,
    LoginFailed = 1,
    TokenReplayDetected = 2,
    AccountLocked = 3,
    PasswordChanged = 4,
    PermissionEscalationAttempt = 5,
    SensitiveDataAccessed = 6
}

public record SecurityEvent(
    SecurityEventType Type,
    SecurityEventSeverity Severity,
    string Description,
    Guid? UserId,
    string? IpAddress,
    string? UserAgent,
    DateTime OccurredOnUtc);

