using Application;
using Infrastructure;
using Presentation;

namespace WebApi.Extensions;

public static class WebApiExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplication()
            .AddInfrastructure()
            .AddPresentation();
    }

    public static void RegisterEndpoints(this WebApplication app)
    {
        app.RunDatabaseMigration();
        app.AddEndpoints();
    }
}
