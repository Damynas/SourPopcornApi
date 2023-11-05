using Application.Auth.Services;
using Application.Directors.Abstractions;
using Application.Directors.Services;
using Application.Movies.Abstractions;
using Application.Movies.Services;
using Application.Ratings.Abstractions;
using Application.Ratings.Services;
using Application.Users.Abstractions;
using Application.Users.Services;
using Application.Votes.Abstractions;
using Application.Votes.Services;
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

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDirectorService, DirectorService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IVoteService, VoteService>();

        return services;
    }
}
