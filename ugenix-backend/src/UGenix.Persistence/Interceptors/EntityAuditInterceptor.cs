using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UGenix.Shared.Abstractions;

namespace UGenix.Persistence.Interceptors;

public class EntityAuditInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUser _currentUser;

    public EntityAuditInterceptor(IDateTimeProvider dateTimeProvider, ICurrentUser currentUser)
    {
        _dateTimeProvider = dateTimeProvider;
        _currentUser = currentUser;
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

        var now = _dateTimeProvider.UtcNow;
        var userId = new UserId(_currentUser.UserId);

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditable.CreatedAtUtc)).CurrentValue = now;
                entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue = userId;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Property(nameof(IAuditable.LastModifiedAtUtc)).CurrentValue = now;
                entry.Property(nameof(IAuditable.LastModifiedBy)).CurrentValue = userId;
            }
        }
    }
}

