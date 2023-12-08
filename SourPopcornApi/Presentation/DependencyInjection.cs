using Application.Abstractions.Services;
using Application.Auth.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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

        AddSwagger(services);

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

    public static void AddSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(option =>
        {
            option.SwaggerEndpoint("/swagger/v1/swagger.json", "SourPopcorn Api");
        });
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SourPopcorn Api",
                Version = "v1"
            });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Bearer authentication with JWT token",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
