using System.Linq.Expressions;
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

    public async Task<T?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>()
            .FirstOrDefaultAsync(e => EF.Property<string>(e, "Email") == email, ct);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate, ct);
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
