using Application.Users.Abstractions;
using Application.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly);
        });

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
