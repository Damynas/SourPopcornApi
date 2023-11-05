using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public static class DatabaseManagementService
{
    public static void MigrationInitialization(IServiceProvider serviceProvider)
    {
        try
        {
            using var serviceScope = serviceProvider.CreateScope();
            // Takes all of our migrations files and apply them against the database in case they are not implemented
            serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
        }
        catch (Exception exception)
        {
            throw new InfrastructureException(exception.Message, exception);
        }
    }
}
