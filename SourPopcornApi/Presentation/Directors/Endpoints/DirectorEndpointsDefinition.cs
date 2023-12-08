using Application.Abstractions.Services;
using Application.Directors.Abstractions;
using Domain.Auth.Constants;
using Domain.Directors.DataTransferObjects.Requests;
using Domain.Directors.DataTransferObjects.Responses;
using Domain.Shared;
using Domain.Shared.Constants;
using Domain.Shared.Paging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Directors.Constants;
using Presentation.Directors.DataTransferObjects;
using Presentation.Directors.Filters;
using Presentation.Shared;
using System.Globalization;
using System.Text.Json;

namespace Presentation.Directors.Endpoints;

public static class DirectorEndpointsDefinition
{
    public static void AddDirectorEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var directors = endpointRouteBuilder.MapGroup("/api").WithTags("Directors").WithOpenApi();

        directors.MapGet("/directors", GetDirectorsAsync)
            .WithName(DirectorEndpointsName.GetDirectors)
            .RequireAuthorization(Policy.User)
            .Produces<EndpointResult<IEnumerable<DirectorResponse>>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        directors.MapGet("/directors/{directorId}", GetDirectorByIdAsync)
            .WithName(DirectorEndpointsName.GetDirectorById)
            .RequireAuthorization(Policy.User)
            .Produces<EndpointResult<DirectorResponse>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");

        directors.MapPost("/directors", CreateDirectorAsync)
            .WithName(DirectorEndpointsName.CreateDirector)
            .AddEndpointFilter<CreateDirectorValidationFilter>()
            .RequireAuthorization(Policy.Moderator)
            .Produces<EndpointResult<DirectorResponse>>(StatusCodes.Status201Created, "application/json")
            .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        directors.MapPut("/directors/{directorId}", UpdateDirectorAsync)
            .WithName(DirectorEndpointsName.UpdateDirector)
            .AddEndpointFilter<UpdateDirectorValidationFilter>()
            .RequireAuthorization(Policy.Moderator)
            .Produces<EndpointResult<DirectorResponse>>(StatusCodes.Status200OK, "application/json")
            .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");

        directors.MapDelete("/directors/{directorId}", DeleteDirectorAsync)
            .WithName(DirectorEndpointsName.DeleteDirector)
            .RequireAuthorization(Policy.Moderator)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");
    }

    private static async Task<IResult> GetDirectorsAsync(HttpContext httpContext,
        [FromServices] IDirectorService directorService, [FromServices] IDirectorMapper directorMapper, [FromServices] ILinkService linkService,
        [FromQuery] int pageNumber = Default.PageNumber, [FromQuery] int pageSize = Default.PageSize, CancellationToken cancellationToken = default)
    {
        var searchParameters = new SearchParameters(pageNumber, pageSize);
        var request = new GetDirectorsRequest(searchParameters);
        var result = await directorService.GetDirectorsAsync(request, cancellationToken);

        var response = directorMapper.ToResponses(result.Value.Items);
        var links = GeneratePagedGetLinks(linkService, result.Value.HasPrevious, result.Value.HasNext, result.Value.CurrentPage, result.Value.PageSize);
        var endpointResult = new EndpointResult<IEnumerable<DirectorResponse>>(response, links.ToList());

        var paginationMetadata = new PaginationMetadata(result.Value.TotalCount, result.Value.PageSize, result.Value.CurrentPage);
        httpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> GetDirectorByIdAsync(
        [FromServices] IDirectorService directorService, [FromServices] IDirectorMapper directorMapper, [FromServices] ILinkService linkService,
        [FromRoute] int directorId, CancellationToken cancellationToken = default)
    {
        var request = new GetDirectorByIdRequest(directorId);
        var result = await directorService.GetDirectorByIdAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = directorMapper.ToResponse(result.Value);
        var links = GenerateGetLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<DirectorResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> CreateDirectorAsync(
        [FromServices] IDirectorService directorService, [FromServices] IDirectorMapper directorMapper, [FromServices] ILinkService linkService,
        [FromBody] CreateDirectorRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new CreateDirectorRequest(requestBody.Name, requestBody.Country, DateTime.Parse(requestBody.BornOn, CultureInfo.InvariantCulture, DateTimeStyles.None));
        var result = await directorService.CreateDirectorAsync(request, cancellationToken);

        var response = directorMapper.ToResponse(result.Value);
        var links = GenerateCreateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<DirectorResponse>(response, links.ToList());

        return TypedResults.CreatedAtRoute(endpointResult, DirectorEndpointsName.GetDirectorById, new { directorId = response.Id });
    }

    private static async Task<IResult> UpdateDirectorAsync(
        [FromServices] IDirectorService directorService, [FromServices] IDirectorMapper directorMapper, [FromServices] ILinkService linkService,
        [FromRoute] int directorId, [FromBody] UpdateDirectorRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new UpdateDirectorRequest(directorId, requestBody.Name, requestBody.Country, DateTime.Parse(requestBody.BornOn, CultureInfo.InvariantCulture, DateTimeStyles.None));
        var result = await directorService.UpdateDirectorAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = directorMapper.ToResponse(result.Value);
        var links = GenerateUpdateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<DirectorResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> DeleteDirectorAsync(
        [FromServices] IDirectorService directorService, [FromRoute] int directorId, CancellationToken cancellationToken = default)
    {
        var request = new DeleteDirectorRequest(directorId);
        var result = await directorService.DeleteDirectorAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        return TypedResults.NoContent();
    }

    private static IEnumerable<Link> GeneratePagedGetLinks(ILinkService linkService, bool hasPrevious, bool hasNext, int pageNumber, int pageSize)
    {
        if (hasPrevious)
            yield return linkService.Generate(DirectorEndpointsName.GetDirectors, new { pageNumber = pageNumber - 1, pageSize }, "self", "GET");
        if (hasNext)
            yield return linkService.Generate(DirectorEndpointsName.GetDirectors, new { pageNumber = pageNumber + 1, pageSize }, "self", "GET");
    }

    private static IEnumerable<Link> GenerateGetLinks(ILinkService linkService, int directorId)
    {
        yield return linkService.Generate(DirectorEndpointsName.UpdateDirector, new { directorId }, "self", "PUT");
        yield return linkService.Generate(DirectorEndpointsName.DeleteDirector, new { directorId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateCreateLinks(ILinkService linkService, int directorId)
    {
        yield return linkService.Generate(DirectorEndpointsName.GetDirectorById, new { directorId }, "self", "GET");
        yield return linkService.Generate(DirectorEndpointsName.UpdateDirector, new { directorId }, "self", "PUT");
        yield return linkService.Generate(DirectorEndpointsName.DeleteDirector, new { directorId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateUpdateLinks(ILinkService linkService, int directorId)
    {
        yield return linkService.Generate(DirectorEndpointsName.GetDirectorById, new { directorId }, "self", "GET");
        yield return linkService.Generate(DirectorEndpointsName.DeleteDirector, new { directorId }, "self", "DELETE");
    }
}
