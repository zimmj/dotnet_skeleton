using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zimmj.Infrastructure.Mongo.Settings;

namespace Zimmj.Integration.Tests.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddConfiguration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.Integration.json")
                .Build());
        });
    }

    public WebApplicationFactory<Program> InjectMongoDbConfigurationSettings(string connectionString)
    {
        return WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(sevices =>
            {
                sevices.Configure<MongoInformation>(
                    opts => { opts.ConnectionString = connectionString; });
            });
        });
    }
}