using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UGem.Infrastructure.Options;
using UGem.Persistence.Interceptors;
using NetTopologySuite.Geometries;

namespace UGem.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        
        // Automatic UUID for all StronglyTypedId equivalents in EF
        // (Handled via ValueConverters in detailed configuration)

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
