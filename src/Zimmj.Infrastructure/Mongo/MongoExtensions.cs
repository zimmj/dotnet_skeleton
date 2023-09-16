using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Zimmj.Infrastructure.Mongo.Houses;
using Zimmj.Infrastructure.Mongo.Interfaces;
using Zimmj.Infrastructure.Mongo.Repositories;
using Zimmj.Infrastructure.Mongo.Settings;

namespace Zimmj.Infrastructure.Mongo;

public static class MongoExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoServices(configuration);

        services.AddHouseRepository(configuration);

        return services;
    }

    private static void AddMongoServices(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = services.GetOptions<MongoInformation>("MongoInformation", configuration);
        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
        services.AddSingleton<IMongoClient>(mongoClient);

        services.AddTransient<IMongoDatabase>(provider =>
        {
            var mongoSetting = provider.GetRequiredService<MongoInformation>();
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoSetting.DatabaseName);
        });

        RegisterConventions();
    }

    private static void RegisterConventions()
    {
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
        BsonSerializer.RegisterSerializer(typeof(decimal?),
            new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
    }

    internal static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(
        this IServiceCollection services,
        string collectionName,
        List<Expression<Func<TEntity, object>>>? descSortingFields = null,
        List<Expression<Func<TEntity, object>>>? ascUniqueFields = null)
        where TEntity : IIdentifiable<TIdentifiable>
        where TIdentifiable : notnull
    {
        services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            descSortingFields?.ForEach(expression => CreateSortIndex(
                provider,
                collectionName,
                Builders<TEntity>.IndexKeys.Descending(expression),
                new CreateIndexOptions()
            ));
            ascUniqueFields?.ForEach(expression => CreateSortIndex(
                provider,
                collectionName,
                Builders<TEntity>.IndexKeys.Ascending(expression),
                new CreateIndexOptions() { Unique = true }
            ));
            return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
        });

        return services;
    }

    private static void CreateSortIndex<TEntity>(
        IServiceProvider serviceProvider,
        string collectionName,
        IndexKeysDefinition<TEntity> indexKeysDefinition,
        CreateIndexOptions indexOptions)
    {
        var mongoClient = serviceProvider.GetService<IMongoClient>();
        var mongoOptions = serviceProvider.GetService<MongoInformation>();

        var collection = mongoClient!.GetDatabase(mongoOptions!.DatabaseName).GetCollection<TEntity>(collectionName);
        var indexModel = new CreateIndexModel<TEntity>(indexKeysDefinition, indexOptions);
        collection.Indexes.CreateOne(indexModel);
    }
}