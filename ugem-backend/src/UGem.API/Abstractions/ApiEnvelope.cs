namespace UGem.API.Abstractions;

public record RequestMetadata(
    string TraceId,
    string CorrelationId,
    DateTime TimestampUtc);

public class ApiEnvelope<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public RequestMetadata Meta { get; init; }

    public ApiEnvelope(T? data, string traceId, string correlationId)
    {
        Success = true;
        Data = data;
        Meta = new RequestMetadata(traceId, correlationId, DateTime.UtcNow);
    }
}
