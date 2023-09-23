using System.Net;
using FluentAssertions;
using Zimmj.Integration.Tests.Fixtures;

namespace Zimmj.Integration.Tests.Houses;

public class TestHouseAgainstMongo2Runner : IClassFixture<CustomWebApplicationFactory>, IClassFixture<Mongo2GoFixture>
{
    private readonly CustomWebApplicationFactory _factory;
    private Mongo2GoFixture _mongoDb;
    
    public TestHouseAgainstMongo2Runner(
        CustomWebApplicationFactory factory,
        Mongo2GoFixture mongoDb)
    {
        _factory = factory;
        _mongoDb = mongoDb;
        
        // Seed data in JSON format
        //_mongoDb.SeedData("./Mongo/Houses/SeedData.json");
    }
    
    [Fact]
    public async void Test1()
    {
        // Arrange
        var client = 
            _factory.InjectMongoDbConfigurationSettings(
                _mongoDb.ConnectionString).CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/houses");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}