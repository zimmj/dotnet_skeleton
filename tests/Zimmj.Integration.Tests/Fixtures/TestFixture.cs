using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Zimmj.Infrastructure.Mongo.Settings;

namespace Zimmj.Integration.Tests.Fixtures;

public class TestFixture<TStartup> : IDisposable where TStartup : class
{
    private readonly string _hostingEnvironment = "Integration";

    private TestFixtureOptions? _testFixtureOptions;
    public Mongo2GoFixture? DatabaseFixture { get; private set; }
    private WebApplicationFactory<TStartup>? _webApplicationFactory;
    private HttpClient? _client;

    public WebApplicationFactory<TStartup> WebApplicationFactory
    {
        get
        {
            return _webApplicationFactory ?? this.ConfigureWebApplicationFactory();
        }
    }

    public HttpClient CreateClient()
    {
        return _client ??= WebApplicationFactory.CreateClient();
    }

    public void ConfigureTestOptions(Action<TestFixtureOptions> configureTestOptions)
    {
        Action<string> configureDatabase = (databaseName) => { DatabaseFixture ??= new Mongo2GoFixture(databaseName); };
        
        Action<Action<IServiceCollection>> configureWebHost = (Action<IServiceCollection> configureTestServices) =>
        {
            _webApplicationFactory = this.ConfigureWebApplicationFactory(configureTestServices);
        };
        
        _testFixtureOptions ??= new TestFixtureOptions(configureDatabase, configureWebHost);
        configureTestOptions.Invoke(_testFixtureOptions);
    }
    
    private WebApplicationFactory<TStartup> ConfigureWebApplicationFactory(Action<IServiceCollection> configureTestServices = null) {
        return new WebApplicationFactory<TStartup>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment(_hostingEnvironment);

            // Before TStartup ConfigureServices.
            builder.ConfigureServices(collection =>
            {
            });

            // After TStartup ConfigureServices.
            builder.ConfigureTestServices(collection =>
            {
                if (DatabaseFixture?.ConnectionString != null)
                {
                    collection.Configure<MongoInformation>(settings =>
                    {
                        settings.ConnectionString = DatabaseFixture.ConnectionString;
                    });
                }
                configureTestServices?.Invoke(collection);
            });
        });
    }

    public void Dispose()
    {
        DatabaseFixture?.Dispose();
        WebApplicationFactory.Dispose();
    }
}