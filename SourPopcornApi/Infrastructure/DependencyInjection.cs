using Application.Abstractions.Data;
using Application.Users.Abstractions;
using Infrastructure.Services;
using Infrastructure.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var connectionString = GetConnectionString();
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(connectionString)
            .UseCamelCaseNamingConvention()
        );

        services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserMapper, UserMapper>();

        return services;
    }

    public static WebApplication RunDatabaseMigration(this WebApplication app)
    {
        DatabaseManagementService.MigrationInitialization(app);

        return app;
    }

    private static string GetConnectionString()
    {
        // Retrieve environment variables
        var host = Environment.GetEnvironmentVariable("SourPopcornDatabase_HOST", EnvironmentVariableTarget.Process);
        var port = Environment.GetEnvironmentVariable("SourPopcornDatabase_PORT", EnvironmentVariableTarget.Process);
        var database = Environment.GetEnvironmentVariable("SourPopcornDatabase_DATABASE", EnvironmentVariableTarget.Process);
        var username = Environment.GetEnvironmentVariable("SourPopcornDatabase_USERNAME", EnvironmentVariableTarget.Process);
        var password = Environment.GetEnvironmentVariable("SourPopcornDatabase_PASSWORD", EnvironmentVariableTarget.Process);

        if (host is null || port is null || database is null || username is null || password is null)
            throw new ArgumentException("Environment variables for a database connection are set incorrectly.");

        if (!int.TryParse(port, out var parsedPort))
            throw new ArgumentException("Port must be an integer.");

        // Construct the connection string
        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = parsedPort,
            Database = database,
            Username = username,
            Password = password,
            IncludeErrorDetail = true,
            IntegratedSecurity = true,
            Pooling = true
        }.ToString();

        return connectionString;
    }
}
