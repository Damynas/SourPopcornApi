using Application.Abstractions.Data;
using Application.Directors.Abstractions;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Application.Users.Abstractions;
using Application.Votes.Abstractions;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infrastructure.Directors;
using Infrastructure.Movies;
using Infrastructure.Ratings;
using Infrastructure.Services;
using Infrastructure.Users;
using Infrastructure.Votes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IWebHostEnvironment environment, IConfigurationManager configuration)
    {
        var connectionString = environment.IsDevelopment() ? GetDevelopmentConnectionString() : GetProductionConnectionString(configuration);
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(connectionString)
            .UseLazyLoadingProxies()
        );

        services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserMapper, UserMapper>();

        services.AddScoped<IDirectorRepository, DirectorRepository>();
        services.AddScoped<IDirectorMapper, DirectorMapper>();

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieMapper, MovieMapper>();

        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IRatingMapper, RatingMapper>();

        services.AddScoped<IVoteRepository, VoteRepository>();
        services.AddScoped<IVoteMapper, VoteMapper>();

        return services;
    }

    public static IServiceProvider RunDatabaseMigration(this IServiceProvider serviceProvider)
    {
        DatabaseManagementService.MigrationInitialization(serviceProvider);

        return serviceProvider;
    }

    private static string GetDevelopmentConnectionString()
    {
        // Retrieve environment variables
        var host = Environment.GetEnvironmentVariable("SourPopcornDatabase_HOST", EnvironmentVariableTarget.Process);
        var port = Environment.GetEnvironmentVariable("SourPopcornDatabase_PORT", EnvironmentVariableTarget.Process);
        var database = Environment.GetEnvironmentVariable("SourPopcornDatabase_DATABASE", EnvironmentVariableTarget.Process);
        var username = Environment.GetEnvironmentVariable("SourPopcornDatabase_USERNAME", EnvironmentVariableTarget.Process);
        var password = Environment.GetEnvironmentVariable("SourPopcornDatabase_PASSWORD", EnvironmentVariableTarget.Process);

        if (host is null || port is null || database is null || username is null || password is null)
            throw new ArgumentException("Variables for a database connection are set incorrectly.");

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
            Pooling = true
        }.ToString();

        return connectionString;
    }

    private static string GetProductionConnectionString(IConfigurationManager configuration)
    {
        var keyVaultUrl = configuration.GetSection("KeyVault:Url");
        var keyVaultTenantId = configuration.GetSection("KeyVault:TenantId");
        var keyVaultClientId = configuration.GetSection("KeyVault:ClientId");
        var keyVaultClientSecret = configuration.GetSection("KeyVault:ClientSecret");

        if (keyVaultUrl.Value is null || keyVaultTenantId.Value is null || keyVaultClientId.Value is null || keyVaultClientSecret.Value is null)
            throw new ArgumentException("Variables for a key vault connection are set incorrectly.");

        var credential = new ClientSecretCredential(keyVaultTenantId.Value, keyVaultClientId.Value, keyVaultClientSecret.Value);
        var client = new SecretClient(new Uri(keyVaultUrl.Value), credential);

        configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

        return client.GetSecret("ProdConnectionString").Value.Value;
    }
}
