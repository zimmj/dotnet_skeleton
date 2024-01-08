using Serilog;
using Zimmj.Core;
using Zimmj.Infrastructure;
using Zimmj.Rest;
using Zimmj.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);
});

builder.Services
    .AddRestLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer()
    .AddCoreLayer();


var app = builder.Build();
var logger = app.Services
    .GetRequiredService<ILogger<Program>>();

logger.LogInformation($"Starting service");
app.StartRestLayer().Run();


public partial class Program
{
}