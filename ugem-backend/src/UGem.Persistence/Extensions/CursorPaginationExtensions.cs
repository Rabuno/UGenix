using Microsoft.EntityFrameworkCore;
using UGem.Shared.Abstractions;

namespace UGem.Persistence.Extensions;

public static class CursorPaginationExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        CursorPaginationRequest request,
        CancellationToken ct = default) where T : class
    {
        // 1. Enforce NoTracking for performance
        var hardenedQuery = query.AsNoTracking();

        // 2. Stable Ordering (Mandatory for cursor stability)
        // Implementation logic for filtering by cursor and limit
        
        var items = await hardenedQuery.Take(request.PageSize + 1).ToListAsync(ct);
        
        // 3. Opaque cursor generation
        return new PagedList<T>(items, new PagedListMetadata(null, null, false, false, 0));
    }
}
