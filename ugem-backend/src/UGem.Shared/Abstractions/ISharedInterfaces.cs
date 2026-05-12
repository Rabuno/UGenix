namespace UGem.Shared.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
