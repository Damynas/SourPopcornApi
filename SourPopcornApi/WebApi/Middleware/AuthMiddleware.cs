using Domain.Auth.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Middleware
{
    public static class AuthMiddleware
    {
        private const string AuthScheme = JwtBearerDefaults.AuthenticationScheme;

        public static void AddAuthMiddleware(this IServiceCollection services, IConfigurationManager configurationManager)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthScheme;
                options.DefaultChallengeScheme = AuthScheme;
                options.DefaultScheme = AuthScheme;
            })
                .AddJwtBearer(options =>
                {
                    var ValidIssuer = configurationManager.GetSection("Jwt:ValidIssuer");
                    var ValidAudience = configurationManager.GetSection("Jwt:ValidAudience");
                    var Secret = configurationManager.GetSection("Jwt:Secret");

                    if (ValidIssuer.Value is null || ValidAudience.Value is null || Secret.Value is null)
                        throw new ArgumentException("Variables for a Jwt usage are set incorrectly.");

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ValidIssuer.Value,
                        ValidAudience = ValidAudience.Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret.Value))
                    };
                    options.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies.TryGetValue(CookieName.AccessToken, out var value) ? value : null;
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy(Policy.Admin, builder =>
                {
                    builder.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(AuthScheme)
                        .RequireClaim(ClaimTypes.Role, Role.Admin);
                })
                .AddPolicy(Policy.Moderator, builder =>
                {
                    builder.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(AuthScheme)
                        .RequireClaim(ClaimTypes.Role, Role.Moderator);
                })
                .AddPolicy(Policy.User, builder =>
                {
                    builder.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(AuthScheme)
                        .RequireClaim(ClaimTypes.Role, Role.User);
                });
        }
    }
}
