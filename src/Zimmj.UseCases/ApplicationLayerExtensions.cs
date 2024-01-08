using Microsoft.Extensions.DependencyInjection;

namespace Zimmj.UseCases;

public static class ApplicationLayerExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationLayerExtensions).Assembly));

        return services;
    }
}