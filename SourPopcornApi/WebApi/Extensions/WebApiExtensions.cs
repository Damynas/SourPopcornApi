using Application;
using Infrastructure;
using Presentation;
using WebApi.Middleware;

namespace WebApi.Extensions;

public static class WebApiExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Environment, builder.Configuration)
            .AddPresentation();

        builder.Services.AddAuthMiddleware(builder.Configuration);
    }

    public static void RegisterServices(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.AddErrorHandlingMiddleware();
        app.AddEndpoints();
    }

    public static void RunServices(this WebApplication app)
    {
        app.Services.RunDatabaseMigration();
    }
}
