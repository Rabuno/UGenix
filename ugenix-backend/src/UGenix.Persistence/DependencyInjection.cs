using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UGenix.Shared.Abstractions;
using UGenix.Persistence.Interceptors;

namespace UGenix.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddScoped<EntityAuditInterceptor>();
        services.AddSingleton<QueryGovernanceInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseNetTopologySuite();
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            })
            .AddInterceptors(
                sp.GetRequiredService<SoftDeleteInterceptor>(),
                sp.GetRequiredService<QueryGovernanceInterceptor>(),
                sp.GetRequiredService<EntityAuditInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

