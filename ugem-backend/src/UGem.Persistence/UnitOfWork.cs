using UGem.Shared.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace UGem.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // 1. Process Domain Events -> Convert to Outbox Messages (Logic here)
        // 2. Save changes
        return await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        return await _dbContext.Database.BeginTransactionAsync(ct);
    }
}
