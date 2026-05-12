using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UGem.Infrastructure.Options;
using UGem.Persistence.Interceptors;
using NetTopologySuite.Geometries;

namespace UGem.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly EntityAuditInterceptor _auditInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        EntityAuditInterceptor auditInterceptor)
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        
        // Automatic UUID for all StronglyTypedId equivalents in EF
        // (Handled via ValueConverters in detailed configuration)

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }
}
