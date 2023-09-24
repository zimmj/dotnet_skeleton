using Microsoft.Extensions.DependencyInjection;

namespace Zimmj.Integration.Tests.Fixtures;

public class TestFixtureOptions
{
    public Action<string> ConfigureDatabase { get; }
    public Action<Action<IServiceCollection>> ConfigureWebHost { get; }
    
    public TestFixtureOptions(Action<string> configureDatabase, Action<Action<IServiceCollection>> configureWebHost)
    {
        ConfigureDatabase = configureDatabase;
        ConfigureWebHost = configureWebHost;
    }
}