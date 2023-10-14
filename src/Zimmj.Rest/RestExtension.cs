using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using NSwag;

namespace Zimmj.Rest;

public static class RestExtensions
{
    public static IServiceCollection AddRestLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAdB2C"));

        IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        IdentityModelEventSource.ShowPII = true;
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
        if (!app.Environment.IsProduction())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}