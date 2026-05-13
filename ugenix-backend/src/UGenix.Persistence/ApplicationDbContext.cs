using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UGenix.Infrastructure.Options;
using UGenix.Persistence.Interceptors;
using NetTopologySuite.Geometries;
using UGenix.Domain.Entities;

namespace UGenix.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<AffiliateLink> AffiliateLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        
        // Automatic UUID for all StronglyTypedId equivalents in EF
        // (Handled via ValueConverters in detailed configuration)

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

