using Serilog;
using Zimmj.Application;
using Zimmj.Core;
using Zimmj.Infrastructure;
using Zimmj.Rest;

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
app.StartRestLayer().Run();


public partial class Program
{
}