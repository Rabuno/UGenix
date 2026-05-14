using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Xunit;
using UGenix.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UGenix.API.IntegrationTests;

public abstract class BaseIntegrationTest : IAsyncLifetime
{
    protected readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgis/postgis:15-3.3")
        .Build();

    protected readonly RedisContainer _redisContainer = new RedisBuilder("redis:7.0-alpine")
        .Build();

    protected HttpClient _client = null!;
    protected IServiceScope _scope = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(_dbContainer.GetConnectionString(), o => o.UseNetTopologySuite()));
                });
            });

        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
    }

    public async Task DisposeAsync()
    {
        if (_scope != null) _scope.Dispose();
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}
