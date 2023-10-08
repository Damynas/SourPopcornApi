using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public static class DatabaseManagementService
{
    public static void MigrationInitialization(WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        // Takes all of our migrations files and apply them against the database in case they are not implemented
        serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
    }
}
