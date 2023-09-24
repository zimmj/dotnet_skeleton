using Microsoft.Extensions.DependencyInjection;

namespace Zimmj.Application;

public static class ApplicationLayerExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationLayerExtensions).Assembly));

        return services;
    }
}