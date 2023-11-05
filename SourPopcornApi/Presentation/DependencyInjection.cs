using Application.Abstractions.Services;
using Application.Auth.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Auth.Endpoints;
using Presentation.Auth.Services;
using Presentation.Directors.Endpoints;
using Presentation.Movies.Endpoints;
using Presentation.Ratings.Endpoints;
using Presentation.Shared.Services;
using Presentation.Users.Endpoints;
using Presentation.Votes.Endpoints;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(PresentationAssemblyReference.Assembly);

        services.AddHttpContextAccessor();

        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IEndpointRouteBuilder AddEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.AddAuthEndpoints();
        routeBuilder.AddUserEndpoints();
        routeBuilder.AddDirectorEndpoints();
        routeBuilder.AddMovieEndpoints();
        routeBuilder.AddRatingEndpoints();
        routeBuilder.AddVoteEndpoints();

        return routeBuilder;
    }
}
