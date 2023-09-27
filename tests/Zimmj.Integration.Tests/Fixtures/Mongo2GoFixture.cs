using Mongo2Go;

namespace Zimmj.Integration.Tests.Fixtures;

public class Mongo2GoFixture : IDisposable
{
    public string ConnectionString { get; }
    private readonly MongoDbRunner _mongoDbRunner;
    public string DatabaseName { get; } = "testDatabase";

    public Mongo2GoFixture(string? databaseName)
    {
        DatabaseName = databaseName ?? DatabaseName;
        _mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true);
        ConnectionString = _mongoDbRunner.ConnectionString;
    }

    public void Dispose()
    {
        _mongoDbRunner.Dispose();
    }
}