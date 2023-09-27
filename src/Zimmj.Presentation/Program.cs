using System.Text.Json.Serialization;
using Zimmj.Application;
using Zimmj.Core;
using Zimmj.Infrastructure;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
    
    builder.Services.AddOpenApiDocument();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddMediatR( cfg => 
        cfg.RegisterServicesFromAssemblyContaining<Program>());

    builder.Services.AddInfrastructureLayer(builder.Configuration);
    builder.Services.AddApplicationLayer();
    builder.Services.AddCoreLayer();
    
    builder.Host.UseSerilog(
        (context, configuration) => 
            configuration.ReadFrom.Configuration(context.Configuration));

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        app.UseOpenApi();
        app.UseSwaggerUi3();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
} catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
} finally
{
    Log.CloseAndFlush();
}


public partial class Program { }