using Application.Abstractions.Services;
using Application.Auth.Abstractions;
using Application.Votes.Abstractions;
using Domain.Auth.Constants;
using Domain.Shared;
using Domain.Shared.Constants;
using Domain.Shared.Paging;
using Domain.Votes.DataTransferObjects.Requests;
using Domain.Votes.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Shared;
using Presentation.Shared.Helpers;
using Presentation.Votes.Constants;
using Presentation.Votes.DataTransferObjects;
using System.Text.Json;

namespace Presentation.Votes.Endpoints;

public static class VoteEndpointsDefinition
{
    public static void AddVoteEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var votes = endpointRouteBuilder.MapGroup("/api/movies/{movieId}/ratings/{ratingId}").WithTags("Votes").WithOpenApi();

        votes.MapGet("/votes", GetMovieRatingVotesAsync)
            .WithName(VoteEndpointsName.GetMovieRatingVotes)
            .RequireAuthorization(Policy.User)
            .Produces<EndpointResult<IEnumerable<VoteResponse>>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        votes.MapGet("/votes/{voteId}", GetMovieRatingVoteByIdAsync)
            .WithName(VoteEndpointsName.GetMovieRatingVoteById)
            .RequireAuthorization(Policy.User)
            .Produces<EndpointResult<VoteResponse>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");

        votes.MapPost("/votes", CreateMovieRatingVoteAsync)
            .WithName(VoteEndpointsName.CreateMovieRatingVote)
            .RequireAuthorization(Policy.User)
            .Produces<EndpointResult<VoteResponse>>(StatusCodes.Status201Created, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status409Conflict);

        votes.MapPut("/votes/{voteId}", UpdateMovieRatingVoteAsync)
            .WithName(VoteEndpointsName.UpdateMovieRatingVote)
            .RequireAuthorization(Policy.User)
            .Produces<EndpointResult<VoteResponse>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");

        votes.MapDelete("/votes/{voteId}", DeleteMovieRatingVoteAsync)
            .WithName(VoteEndpointsName.DeleteMovieRatingVote)
            .RequireAuthorization(Policy.User)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain");
    }

    private static async Task<IResult> GetMovieRatingVotesAsync(HttpContext httpContext,
        [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromRoute] int ratingId, [FromQuery] int pageNumber = Default.PageNumber, [FromQuery] int pageSize = Default.PageSize, CancellationToken cancellationToken = default)
    {
        var searchParameters = new SearchParameters(pageNumber, pageSize);
        var request = new GetMovieRatingVotesRequest(movieId, ratingId, searchParameters);
        var result = await voteService.GetMovieRatingVotesAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = voteMapper.ToResponses(result.Value.Items);
        var links = GeneratePagedGetLinks(linkService, movieId, ratingId, result.Value.HasPrevious, result.Value.HasNext, result.Value.CurrentPage, result.Value.PageSize);
        var endpointResult = new EndpointResult<IEnumerable<VoteResponse>>(response, links.ToList());

        var paginationMetadata = new PaginationMetadata(result.Value.TotalCount, result.Value.PageSize, result.Value.CurrentPage);
        httpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> GetMovieRatingVoteByIdAsync(
        [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromRoute] int ratingId, [FromRoute] int voteId, CancellationToken cancellationToken = default)
    {
        var request = new GetMovieRatingVoteByIdRequest(movieId, ratingId, voteId);
        var result = await voteService.GetMovieRatingVoteByIdAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = voteMapper.ToResponse(result.Value);
        var links = GenerateGetLinks(linkService, movieId, ratingId, response.Id);
        var endpointResult = new EndpointResult<VoteResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> CreateMovieRatingVoteAsync(HttpContext httpContext,
        [FromServices] ITokenService tokenService, [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromRoute] int ratingId, [FromBody] CreateMovieRatingVoteRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var accessToken = CookieHelper.GetCookie(httpContext, CookieName.AccessToken);
        if (accessToken is null)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var userId = tokenService.GetUserId(accessToken);
        if (!userId.HasValue)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var request = new CreateMovieRatingVoteRequest(userId.Value, movieId, ratingId, requestBody.IsPositive);
        var result = await voteService.CreateMovieRatingVoteAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code switch
            {
                ErrorCode.NullValue => TypedResults.NotFound(result.Error.Message),
                ErrorCode.Conflict => TypedResults.Conflict(result.Error.Message),
                _ => TypedResults.Problem("Failed result error value is incorrect.")
            };

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = voteMapper.ToResponse(result.Value);
        var links = GenerateCreateLinks(linkService, movieId, ratingId, response.Id);
        var endpointResult = new EndpointResult<VoteResponse>(response, links.ToList());

        return TypedResults.CreatedAtRoute(endpointResult, VoteEndpointsName.GetMovieRatingVoteById, new { voteId = response.Id });
    }

    private static async Task<IResult> UpdateMovieRatingVoteAsync(HttpContext httpContext,
        [FromServices] ITokenService tokenService, [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromRoute] int ratingId, [FromRoute] int voteId, [FromBody] UpdateMovieRatingVoteRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var accessToken = CookieHelper.GetCookie(httpContext, CookieName.AccessToken);
        if (accessToken is null)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var userId = tokenService.GetUserId(accessToken);
        if (!userId.HasValue)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var roles = tokenService.GetRoles(accessToken);
        if (roles is null)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var request = new UpdateMovieRatingVoteRequest(userId.Value, roles, movieId, ratingId, voteId, requestBody.IsPositive);
        var result = await voteService.UpdateMovieRatingVoteAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code switch
            {
                ErrorCode.NullValue => TypedResults.NotFound(result.Error.Message),
                ErrorCode.Forbidden => TypedResults.Conflict(result.Error.Message),
                _ => TypedResults.Problem("Failed result error value is incorrect.")
            };

        if (result.Value is null)
            return TypedResults.Problem("Successful result value cannot be null.");

        var response = voteMapper.ToResponse(result.Value);
        var links = GenerateUpdateLinks(linkService, movieId, ratingId, response.Id);
        var endpointResult = new EndpointResult<VoteResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> DeleteMovieRatingVoteAsync(HttpContext httpContext,
        [FromServices] ITokenService tokenService, [FromServices] IVoteService voteService,
        [FromRoute] int movieId, [FromRoute] int ratingId, [FromRoute] int voteId, CancellationToken cancellationToken = default)
    {
        var accessToken = CookieHelper.GetCookie(httpContext, CookieName.AccessToken);
        if (accessToken is null)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var userId = tokenService.GetUserId(accessToken);
        if (!userId.HasValue)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var roles = tokenService.GetRoles(accessToken);
        if (roles is null)
            return TypedResults.UnprocessableEntity("Access token is not valid.");

        var request = new DeleteMovieRatingVoteRequest(userId.Value, roles, movieId, ratingId, voteId);
        var result = await voteService.DeleteMovieRatingVoteAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code switch
            {
                ErrorCode.NullValue => TypedResults.NotFound(result.Error.Message),
                ErrorCode.Forbidden => TypedResults.Forbid(),
                _ => TypedResults.Problem("Failed result error value is incorrect.")
            };

        return TypedResults.NoContent();
    }

    private static IEnumerable<Link> GeneratePagedGetLinks(ILinkService linkService, int movieId, int ratingId, bool hasPrevious, bool hasNext, int pageNumber, int pageSize)
    {
        if (hasPrevious)
            yield return linkService.Generate(VoteEndpointsName.GetMovieRatingVotes, new { movieId, ratingId, pageNumber = pageNumber - 1, pageSize }, "self", "GET");
        if (hasNext)
            yield return linkService.Generate(VoteEndpointsName.GetMovieRatingVotes, new { movieId, ratingId, pageNumber = pageNumber + 1, pageSize }, "self", "GET");
    }

    private static IEnumerable<Link> GenerateGetLinks(ILinkService linkService, int movieId, int ratingId, int voteId)
    {
        yield return linkService.Generate(VoteEndpointsName.UpdateMovieRatingVote, new { movieId, ratingId, voteId }, "self", "PUT");
        yield return linkService.Generate(VoteEndpointsName.DeleteMovieRatingVote, new { movieId, ratingId, voteId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateCreateLinks(ILinkService linkService, int movieId, int ratingId, int voteId)
    {
        yield return linkService.Generate(VoteEndpointsName.GetMovieRatingVoteById, new { movieId, ratingId, voteId }, "self", "GET");
        yield return linkService.Generate(VoteEndpointsName.UpdateMovieRatingVote, new { movieId, ratingId, voteId }, "self", "PUT");
        yield return linkService.Generate(VoteEndpointsName.DeleteMovieRatingVote, new { movieId, ratingId, voteId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateUpdateLinks(ILinkService linkService, int movieId, int ratingId, int voteId)
    {
        yield return linkService.Generate(VoteEndpointsName.GetMovieRatingVoteById, new { movieId, ratingId, voteId }, "self", "GET");
        yield return linkService.Generate(VoteEndpointsName.DeleteMovieRatingVote, new { movieId, ratingId, voteId }, "self", "DELETE");
    }
}
