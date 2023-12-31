using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using NSwag;

namespace Zimmj.Rest;

public static class RestExtensions
{
    public static IServiceCollection AddRestLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAdB2C"));

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument(options =>
        {
            options.PostProcess = document =>
            {
                document.Info = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "House example API",
                    Description = "A simple example ASP.NET Core Web API",
                };
            };
            options.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme()
            {
                Type = OpenApiSecuritySchemeType.Http,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "JWT token for authentication and authorization against B2C.",
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
        });
        return services;
    }

    public static WebApplication StartRestLayer(this WebApplication app)
    {
        var logger = app.Services
            .GetRequiredService<ILogger<RestExtension>>();
        
        if (!app.Environment.IsProduction())
        {
            logger.LogInformation("Create swagger endpoints");
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
       

        logger.LogInformation($"Starting service");
        return app;
    }
}

public partial class RestExtension
{
}