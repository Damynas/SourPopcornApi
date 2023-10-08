using Application.Abstractions.Services;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Shared.Services;
using Presentation.Users.Endpoints;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(PresentationAssemblyReference.Assembly);

        services.AddHttpContextAccessor();

        services.AddScoped<ILinkService, LinkService>();

        return services;
    }

    public static WebApplication AddEndpoints(this WebApplication app)
    {
        app.AddUserEndpoints();

        return app;
    }
}
