using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace UGem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Redis, Cloudinary, etc. will be added here
        return services;
    }
}
