using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace UGem.Persistence.Interceptors;

public class UtcDateTimeInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken ct = default)
    {
        var entries = eventData.Context!.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            var properties = entry.Metadata.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

            foreach (var property in properties)
            {
                var val = entry.Property(property.Name).CurrentValue;
                if (val is DateTime dt)
                {
                    entry.Property(property.Name).CurrentValue = dt.Kind == DateTimeKind.Unspecified 
                        ? DateTime.SpecifyKind(dt, DateTimeKind.Utc) 
                        : dt.ToUniversalTime();
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
