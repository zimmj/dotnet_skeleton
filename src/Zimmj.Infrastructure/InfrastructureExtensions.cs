using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zimmj.Infrastructure.Mongo;

namespace Zimmj.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMongoDb(configuration);
        return services;
    }

    public static TModel GetOptions<TModel>(this IServiceCollection services, string sectionName,
        IConfiguration configuration)
        where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(sectionName).Bind(model);
        return model;
    }
}