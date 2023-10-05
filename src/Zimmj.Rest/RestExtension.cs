
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Zimmj.Rest;

public static class RestExtensions
{
    public static IServiceCollection AddRestLayer(this IServiceCollection services)
    {
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        services.AddOpenApiDocument();
        services.AddEndpointsApiExplorer();
        
        return services;
    }
    
    public static WebApplication StartRestLayer(this WebApplication app)
    {
        if (!app.Environment.IsProduction())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}