using Mongo2Go;
using MongoDB.Driver;
using Zimmj.Infrastructure.Mongo.Houses;

namespace Zimmj.Integration.Tests.Fixtures;

public class Mongo2GoFixture : IDisposable
{
    public MongoClient Client { get; }
    public IMongoDatabase Database { get; }
    public string ConnectionString { get; }
    private readonly MongoDbRunner _mongoDbRunner;
    private readonly string _databaseName = "testDatabase";
    public IMongoCollection<HouseDocument> DataBoundCollection { get; }

    public Mongo2GoFixture(string? databaseName)
    {
        _databaseName = databaseName ?? _databaseName;
        _mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true);
        ConnectionString = _mongoDbRunner.ConnectionString;
        Client = new MongoClient(ConnectionString);
        Database = Client.GetDatabase(databaseName);
        DataBoundCollection = Database.GetCollection<HouseDocument>("Houses");
    }

    public void Dispose()
    {
        _mongoDbRunner.Dispose();
    }
}