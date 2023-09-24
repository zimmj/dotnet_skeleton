using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zimmj.Core.Houses;

namespace Zimmj.Infrastructure.Mongo.Houses;

public static class HousesExtensions
{
    public static IServiceCollection AddHouseRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var houseStoreDatabaseSettings =
            services.GetOptions<HouseStoreDatabaseSettings>("HouseStoreDatabaseSettings", configuration);
        services.AddMongoRepository<HouseDocument, string>(
            houseStoreDatabaseSettings.CollectionName);
        services.AddScoped<IHouseRepository, HouseRepository>();

        return services;
    }
}