using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UGenix.Shared.Abstractions;

namespace UGenix.Persistence.Interceptors;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAtUtc { get; }
    void Delete(DateTime now);
}

public interface IAuditable
{
    DateTime CreatedAtUtc { get; }
    UserId? CreatedBy { get; }
    DateTime? LastModifiedAtUtc { get; }
    UserId? LastModifiedBy { get; }
}

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public SoftDeleteInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.Delete(_dateTimeProvider.UtcNow);
            }
        }
    }
}

