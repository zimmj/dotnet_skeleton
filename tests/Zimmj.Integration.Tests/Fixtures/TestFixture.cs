using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMotions.Fake.Authentication.JwtBearer;
using Zimmj.Core.Houses;

namespace Zimmj.Integration.Tests.Fixtures;

public class TestFixture<TStartup> : IDisposable where TStartup : class
{
    private readonly string _hostingEnvironment = "Integration";

    private TestFixtureOptions? _testFixtureOptions;
    private Mongo2GoFixture? DatabaseFixture { get; set; }
    private WebApplicationFactory<TStartup>? _webApplicationFactory;
    private HttpClient? _client;

    private WebApplicationFactory<TStartup> WebApplicationFactory =>
        _webApplicationFactory ?? ConfigureWebApplicationFactory();

    public HttpClient CreateClient()
    {
        return _client ??= WebApplicationFactory.CreateClient();
    }

    private bool _seeded = false;
    public async void Seed(List<House> houses)
    {
        if(_seeded)
            return;
        using var scope = WebApplicationFactory.Server.Services.CreateScope();
        var houseRepository = scope.ServiceProvider.GetRequiredService<IHouseRepository>();
        await houseRepository.AddManyAsync(houses);
        _seeded = true;
    }

    public void ConfigureTestOptions(Action<TestFixtureOptions> configureTestOptions)
    {
        Action<string> configureDatabase = (databaseName) => { DatabaseFixture ??= new Mongo2GoFixture(databaseName); };

        Action<Action<IServiceCollection>> configureWebHost = (Action<IServiceCollection> configureTestServices) =>
        {
            _webApplicationFactory = ConfigureWebApplicationFactory(configureTestServices);
        };

        _testFixtureOptions ??= new TestFixtureOptions(configureDatabase, configureWebHost);
        configureTestOptions.Invoke(_testFixtureOptions);
    }

    private WebApplicationFactory<TStartup> ConfigureWebApplicationFactory(
        Action<IServiceCollection> configureTestServices = null)
    {
        var testConfigurationBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new List<KeyValuePair<string, string?>>
                {
                    new("MongoInformation:ConnectionString", DatabaseFixture?.ConnectionString ?? ""),
                    new("MongoInformation:DatabaseName", DatabaseFixture?.DatabaseName ?? "")
                }).Build();


        return new WebApplicationFactory<TStartup>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(_hostingEnvironment)
                    .UseConfiguration(testConfigurationBuilder)
                    .ConfigureServices(collection => { });
                builder.UseEnvironment(_hostingEnvironment);

                // Before TStartup ConfigureServices.
                builder.ConfigureServices(collection =>
                {
                    collection.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
                });

                // After TStartup ConfigureServices.
                builder.ConfigureTestServices(collection => { });
            });
    }

    public void Dispose()
    {
        DatabaseFixture?.Dispose();
        WebApplicationFactory.Dispose();
    }
}