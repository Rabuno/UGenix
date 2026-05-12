namespace UGem.Persistence.Outbox;

public class OutboxProcessorOptions
{
    public int MaxRetryCount { get; set; } = 10;
    public int BatchSize { get; set; } = 20;
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);
}

public interface IOutboxProcessor
{
    Task ProcessBatchAsync(CancellationToken ct = default);
}

// Logic handles moving failed messages to "Dead-letter" state 
// after MaxRetryCount is reached.
