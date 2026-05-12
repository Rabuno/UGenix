using Microsoft.EntityFrameworkCore;
using UGem.Shared.Abstractions;

namespace UGem.Persistence.Interceptors;

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
    // Automatically intercept DELETE commands and convert them to UPDATE IsDeleted = true
}
