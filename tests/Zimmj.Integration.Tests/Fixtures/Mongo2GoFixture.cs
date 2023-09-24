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

    public Mongo2GoFixture(string databaseName)
    {
        _databaseName = databaseName ?? _databaseName;
        _mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true);
        ConnectionString = _mongoDbRunner.ConnectionString;
        Client = new MongoClient(ConnectionString);
        Database = Client.GetDatabase(databaseName);
        DataBoundCollection = Database.GetCollection<HouseDocument>("houses");
    }

    public void SeedData(string file)
    {
        var documentCount = DataBoundCollection.CountDocuments(Builders<HouseDocument>.Filter.Empty);
        if (documentCount == 0)
        {
            _mongoDbRunner.Import(_databaseName, "house", GetFilePath(file), true);
        }
    }
    
    private string GetFilePath(string file)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var directoryInfo = new DirectoryInfo(currentDirectory);
        var path = Path.Combine(directoryInfo.Parent?.Parent?.Parent?.FullName ?? string.Empty, "Data", file);
        return path;
    }
    
    public void Dispose()
    {
        _mongoDbRunner.Dispose();
    }
}