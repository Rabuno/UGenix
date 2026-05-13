using Microsoft.EntityFrameworkCore;
using UGenix.Shared.Abstractions;

namespace UGenix.Persistence;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id }, ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, ct);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}
