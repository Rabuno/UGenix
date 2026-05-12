using Microsoft.Extensions.DependencyInjection;

namespace UGem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR, FluentValidation, etc. will be added here in future phases
        return services;
    }
}
