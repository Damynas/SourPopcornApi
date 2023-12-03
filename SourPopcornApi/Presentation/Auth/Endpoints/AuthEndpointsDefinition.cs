using Application.Auth.Abstractions;
using Application.Auth.Services;
using Application.Users.Abstractions;
using Domain.Auth.Constants;
using Domain.Auth.DataTransferObjects.Requests;
using Domain.Auth.DataTransferObjects.Responses;
using Domain.Shared.Constants;
using Domain.Users.DataTransferObjects.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Auth.Constants;
using Presentation.Auth.DataTransferObjects;
using Presentation.Auth.Filters;
using Presentation.Shared.Helpers;

namespace Presentation.Auth.Endpoints
{
    public static class AuthEndpointsDefinition
    {
        public static void AddAuthEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var auth = endpointRouteBuilder.MapGroup("/api").WithTags("Auth");
            auth.MapPost("/auth/login", LoginAsync)
                .WithName(AuthEndpointName.Login)
                .AddEndpointFilter<LoginValidationFilter>()
                .AllowAnonymous();
            auth.MapPost("/auth/logout", LogoutAsync)
                .WithName(AuthEndpointName.Logout)
                .RequireAuthorization(Policy.User);
            auth.MapPost("/auth/register", RegisterAsync)
                .WithName(AuthEndpointName.Register)
                .AddEndpointFilter<RegisterValidationFilter>()
                .AllowAnonymous();

            auth.MapPost("/auth/refresh_access_token", RefreshAccessTokenAsync)
                .WithName(AuthEndpointName.RefreshAccessToken)
                .AllowAnonymous();
            auth.MapPost("/auth/ping", PingAsync)
                .WithName(AuthEndpointName.Ping)
                .AllowAnonymous();
        }

        private static async Task<IResult> LoginAsync(HttpContext httpContext,
            [FromServices] IAuthService authService, [FromServices] ITokenService tokenService,
            [FromBody] LoginRequestBody requestBody, CancellationToken cancellationToken = default)
        {
            var request = new LoginRequest(requestBody.Username, requestBody.Password);
            var result = await authService.LoginAsync(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

            if (result.Value is null)
                return TypedResults.Problem("Successful result value cannot be null.");

            var accessToken = tokenService.GenerateAccessToken(result.Value);
            var refreshToken = tokenService.GenerateRefreshToken(result.Value);

            CookieHelper.SetCookie(httpContext, CookieName.AccessToken, accessToken, 5);
            CookieHelper.SetCookie(httpContext, CookieName.RefreshToken, refreshToken, 24 * 60);

            return TypedResults.Ok(new TokenResponse(accessToken, refreshToken));
        }

        private static async Task<IResult> LogoutAsync(HttpContext httpContext,
            [FromServices] IAuthService authService, [FromServices] ITokenService tokenService,
            CancellationToken cancellationToken = default)
        {
            var accessToken = CookieHelper.GetCookie(httpContext, CookieName.AccessToken);
            if (accessToken is null)
                return TypedResults.Unauthorized();

            var userId = tokenService.GetUserId(accessToken);
            if (!userId.HasValue)
                return TypedResults.Unauthorized();

            var request = new LogoutRequest(userId.Value);
            var result = await authService.LogoutAsync(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

            CookieHelper.RemoveCookie(httpContext, CookieName.AccessToken);
            CookieHelper.RemoveCookie(httpContext, CookieName.RefreshToken);

            return TypedResults.NoContent();
        }

        private static async Task<IResult> RegisterAsync(
            [FromServices] IAuthService authService, [FromServices] IUserMapper userMapper,
            [FromBody] RegisterRequestBody requestBody, CancellationToken cancellationToken = default)
        {
            var request = new RegisterRequest(requestBody.Username, requestBody.Password, requestBody.DisplayName);
            var result = await authService.RegisterAsync(request, cancellationToken);

            var response = userMapper.ToResponse(result.Value);

            return TypedResults.CreatedAtRoute(response, AuthEndpointName.Login);
        }

        private static async Task<IResult> RefreshAccessTokenAsync(HttpContext httpContext,
            [FromServices] ITokenService tokenService, [FromServices] IUserService userService,
            CancellationToken cancellationToken = default)
        {
            var refreshToken = CookieHelper.GetCookie(httpContext, CookieName.RefreshToken);
            if (refreshToken is null)
                return TypedResults.Unauthorized();

            var userId = tokenService.GetUserId(refreshToken);
            if (!userId.HasValue)
                return TypedResults.Unauthorized();

            var request = new GetUserByIdRequest(userId.Value);
            var result = await userService.GetUserByIdAsync(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.Code == ErrorCode.NullValue ? TypedResults.Unauthorized() : TypedResults.Problem("Failed result error value is incorrect.");

            if (result.Value is null)
                return TypedResults.Problem("Successful result value cannot be null.");

            if (result.Value.ForceLogin)
                return TypedResults.Unauthorized();

            var accessToken = tokenService.GenerateAccessToken(result.Value);
            refreshToken = tokenService.GenerateRefreshToken(result.Value);

            CookieHelper.SetCookie(httpContext, CookieName.AccessToken, accessToken, 5);
            CookieHelper.SetCookie(httpContext, CookieName.RefreshToken, refreshToken, 24 * 60);

            return TypedResults.Ok(new TokenResponse(accessToken, refreshToken));
        }

        private static async Task<IResult> PingAsync(HttpContext httpContext,
            [FromServices] ITokenService tokenService, [FromServices] IUserService userService,
            CancellationToken cancellationToken = default)
        {
            var accessToken = CookieHelper.GetCookie(httpContext, CookieName.AccessToken);
            if (accessToken is null)
                return TypedResults.Unauthorized();

            var userId = tokenService.GetUserId(accessToken);
            if (!userId.HasValue)
                return TypedResults.Unauthorized();

            var roles = tokenService.GetRoles(accessToken);
            if (roles is null || !roles.Any())
                return TypedResults.Unauthorized();

            var request = new GetUserByIdRequest(userId.Value);
            var result = await userService.GetUserByIdAsync(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.Code == ErrorCode.NullValue ? TypedResults.Unauthorized() : TypedResults.Problem("Failed result error value is incorrect.");

            if (result.Value is null)
                return TypedResults.Problem("Successful result value cannot be null.");

            if (result.Value.ForceLogin)
                return TypedResults.Unauthorized();

            return TypedResults.Ok(new PingResponse(userId.Value, roles));
        }
    }
}
