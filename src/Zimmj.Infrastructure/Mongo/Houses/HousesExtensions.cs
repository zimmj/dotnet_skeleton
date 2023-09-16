using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zimmj.Infrastructure.Mongo.Houses.Documents;
using Zimmj.Infrastructure.Mongo.Houses.Settings;

namespace Zimmj.Infrastructure.Mongo.Houses;

public static class HousesExtensions
{
    public static IServiceCollection AddHouseRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var houseStoreDatabaseSettings =
            services.GetOptions<HouseStoreDatabaseSettings>("HouseStoreDatabaseSettings", configuration);
        services.AddMongoRepository<HouseDocument, string>(
            houseStoreDatabaseSettings.CollectionName);

        return services;
    }
}