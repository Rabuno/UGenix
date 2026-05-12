using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UGenix.Infrastructure.Options;
using UGenix.Persistence.Interceptors;
using NetTopologySuite.Geometries;

namespace UGenix.Persistence;

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

