﻿using Application.Abstractions.Services;
using Application.Users.Abstractions;
using Domain.Auth.Constants;
using Domain.Shared;
using Domain.Shared.Constants;
using Domain.Shared.Paging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Shared;
using Presentation.Users.Constants;
using Presentation.Users.DataTransferObjects;
using Presentation.Users.Filters;
using System.Text.Json;

namespace Presentation.Users.Endpoints;

public static class UserEndpointsDefinition
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var users = endpointRouteBuilder.MapGroup("/api").WithTags("Users").WithOpenApi();

        users.MapGet("/users", GetUsersAsync)
            .WithName(UserEndpointsName.GetUsers)
            .RequireAuthorization(Policy.Admin)
            .Produces<EndpointResult<IEnumerable<UserResponse>>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        users.MapGet("/users/{userId}", GetUserByIdAsync)
            .WithName(UserEndpointsName.GetUserById)
            .RequireAuthorization(Policy.Admin)
            .Produces<EndpointResult<UserResponse>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");

        users.MapPost("/users", CreateUserAsync)
            .WithName(UserEndpointsName.CreateUser)
            .AddEndpointFilter<CreateUserValidationFilter>()
            .RequireAuthorization(Policy.Admin)
            .Produces<EndpointResult<UserResponse>>(StatusCodes.Status201Created, "application/json")
            .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        users.MapPut("/users/{userId}", UpdateUserAsync)
            .WithName(UserEndpointsName.UpdateUser)
            .AddEndpointFilter<UpdateUserValidationFilter>()
            .RequireAuthorization(Policy.Admin)
            .Produces<EndpointResult<UserResponse>>(StatusCodes.Status200OK, "application/json")
            .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");

        users.MapDelete("/users/{userId}", DeleteUserAsync)
            .WithName(UserEndpointsName.DeleteUser)
            .RequireAuthorization(Policy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");
    }

    private static async Task<IResult> GetUsersAsync(HttpContext httpContext,
        [FromServices] IUserService userService, [FromServices] IUserMapper userMapper, [FromServices] ILinkService linkService,
        [FromQuery] int pageNumber = Default.PageNumber, [FromQuery] int pageSize = Default.PageSize, CancellationToken cancellationToken = default)
    {
        var searchParameters = new SearchParameters(pageNumber, pageSize);
        var request = new GetUsersRequest(searchParameters);
        var result = await userService.GetUsersAsync(request, cancellationToken);

        var response = userMapper.ToResponses(result.Value.Items);
        var links = GeneratePagedGetLinks(linkService, result.Value.HasPrevious, result.Value.HasNext, result.Value.CurrentPage, result.Value.PageSize);
        var endpointResult = new EndpointResult<IEnumerable<UserResponse>>(response, links.ToList());

        var paginationMetadata = new PaginationMetadata(result.Value.TotalCount, result.Value.PageSize, result.Value.CurrentPage);
        httpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> GetUserByIdAsync(
        [FromServices] IUserService userService, [FromServices] IUserMapper userMapper, [FromServices] ILinkService linkService,
        [FromRoute] int userId, CancellationToken cancellationToken = default)
    {
        var request = new GetUserByIdRequest(userId);
        var result = await userService.GetUserByIdAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = userMapper.ToResponse(result.Value);
        var links = GenerateGetLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<UserResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> CreateUserAsync(
        [FromServices] IUserService userService, [FromServices] IUserMapper userMapper, [FromServices] ILinkService linkService,
        [FromBody] CreateUserRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new CreateUserRequest(requestBody.Username, requestBody.Password, requestBody.DisplayName, requestBody.Roles);
        var result = await userService.CreateUserAsync(request, cancellationToken);

        var response = userMapper.ToResponse(result.Value);
        var links = GenerateCreateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<UserResponse>(response, links.ToList());

        return TypedResults.CreatedAtRoute(endpointResult, UserEndpointsName.GetUserById, new { userId = response.Id });
    }

    private static async Task<IResult> UpdateUserAsync(
        [FromServices] IUserService userService, [FromServices] IUserMapper userMapper, [FromServices] ILinkService linkService,
        [FromRoute] int userId, [FromBody] UpdateUserRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new UpdateUserRequest(userId, requestBody.DisplayName, requestBody.Roles);
        var result = await userService.UpdateUserAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = userMapper.ToResponse(result.Value);
        var links = GenerateUpdateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<UserResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> DeleteUserAsync(
        [FromServices] IUserService userService, [FromRoute] int userId, CancellationToken cancellationToken = default)
    {
        var request = new DeleteUserRequest(userId);
        var result = await userService.DeleteUserAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        return TypedResults.NoContent();
    }

    private static IEnumerable<Link> GeneratePagedGetLinks(ILinkService linkService, bool hasPrevious, bool hasNext, int pageNumber, int pageSize)
    {
        if (hasPrevious)
            yield return linkService.Generate(UserEndpointsName.GetUsers, new { pageNumber = pageNumber - 1, pageSize }, "self", "GET");
        if (hasNext)
            yield return linkService.Generate(UserEndpointsName.GetUsers, new { pageNumber = pageNumber + 1, pageSize }, "self", "GET");
    }

    private static IEnumerable<Link> GenerateGetLinks(ILinkService linkService, int userId)
    {
        yield return linkService.Generate(UserEndpointsName.UpdateUser, new { userId }, "self", "PUT");
        yield return linkService.Generate(UserEndpointsName.DeleteUser, new { userId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateCreateLinks(ILinkService linkService, int userId)
    {
        yield return linkService.Generate(UserEndpointsName.GetUserById, new { userId }, "self", "GET");
        yield return linkService.Generate(UserEndpointsName.UpdateUser, new { userId }, "self", "PUT");
        yield return linkService.Generate(UserEndpointsName.DeleteUser, new { userId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateUpdateLinks(ILinkService linkService, int userId)
    {
        yield return linkService.Generate(UserEndpointsName.GetUserById, new { userId }, "self", "GET");
        yield return linkService.Generate(UserEndpointsName.DeleteUser, new { userId }, "self", "DELETE");
    }
}
