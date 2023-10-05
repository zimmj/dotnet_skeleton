using Serilog;
using Zimmj.Application;
using Zimmj.Core;
using Zimmj.Infrastructure;
using Zimmj.Rest;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRestLayer()
        .AddInfrastructureLayer(builder.Configuration)
        .AddApplicationLayer()
        .AddCoreLayer();

    var app = builder.Build();
    app.StartRestLayer().Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
}