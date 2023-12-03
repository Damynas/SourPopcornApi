using Application;
using Infrastructure;
using Presentation;
using WebApi.Middleware;

namespace WebApi.Extensions;

public static class WebApiExtensions
{
    private const string CorsPolicyName = "SourPopcornCorsPolicy";

    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Environment, builder.Configuration)
            .AddPresentation();

        // Add CORS
        var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins");
        if (allowedOrigins.Value is null)
            throw new ArgumentException("CORS policy configuration section is incorrect.");
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicyName,
                builder => builder
                    .WithOrigins(allowedOrigins.Value)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        builder.Services.AddAuthMiddleware(builder.Configuration);
    }

    public static void RegisterServices(this WebApplication app)
    {
        app.AddErrorHandlingMiddleware();

        app.UseHttpsRedirection();
        app.UseCors(CorsPolicyName);

        app.UseAuthentication();
        app.UseAuthorization();

        app.AddEndpoints();
    }

    public static void RunServices(this WebApplication app)
    {
        app.Services.RunDatabaseMigration();
    }
}
