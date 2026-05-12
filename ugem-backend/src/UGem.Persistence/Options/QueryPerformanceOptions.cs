namespace UGem.Persistence.Options;

public class QueryPerformanceOptions
{
    public int DefaultQueryTimeoutSeconds { get; set; } = 30;
    public int SlowQueryThresholdMilliseconds { get; set; } = 500;
}
