namespace UGenix.Shared.Abstractions;

public interface IIdempotentRequest
{
    Guid IdempotencyKey { get; }
}

public interface IVersionable
{
    byte[] RowVersion { get; }
}

