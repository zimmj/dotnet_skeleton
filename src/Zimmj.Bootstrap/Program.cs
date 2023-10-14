using Zimmj.Application;
using Zimmj.Core;
using Zimmj.Infrastructure;
using Zimmj.Rest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRestLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer()
    .AddCoreLayer();


var app = builder.Build();
app.StartRestLayer().Run();


public partial class Program
{
}