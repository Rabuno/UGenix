namespace UGem.Shared.Abstractions;

public interface IIdempotentRequest
{
    Guid IdempotencyKey { get; }
}

public interface IVersionable
{
    byte[] RowVersion { get; }
}

public record ConcurrencyMetadata(byte[] RowVersion);

public static class IdempotencyConstants
{
    public const string HeaderName = "X-Idempotency-Key";
}

public abstract class BaseVersionedEntity<TId> : BaseEntity<TId>, IVersionable 
    where TId : struct
{
    protected BaseVersionedEntity(TId id) : base(id) { }
    
    // EF Core RowVersion (xmin or dedicated byte[])
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();
}
