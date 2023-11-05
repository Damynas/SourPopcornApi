using Application.Abstractions.Services;
using Application.Auth.Abstractions;
using Application.Ratings.Abstractions;
using Domain.Auth.Constants;
using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.DataTransferObjects.Responses;
using Domain.Shared;
using Domain.Shared.Constants;
using Domain.Shared.Paging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Ratings.Constants;
using Presentation.Ratings.DataTransferObjects;
using Presentation.Ratings.Filters;
using Presentation.Shared;
using Presentation.Shared.Helpers;
using System.Text.Json;

namespace Presentation.Ratings.Endpoints;

public static class RatingEndpointsDefinition
{
    public static void AddRatingEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ratings = endpointRouteBuilder.MapGroup("/api/movies/{movieId}").WithTags("Ratings");
        ratings.MapGet("/ratings", GetMovieRatingsAsync)
            .WithName(RatingEndpointsName.GetMovieRatings)
            .RequireAuthorization(Policy.UserOnly);
        ratings.MapGet("/ratings/{ratingId}", GetMovieRatingByIdAsync)
            .WithName(RatingEndpointsName.GetMovieRatingById)
            .RequireAuthorization(Policy.UserOnly);
        ratings.MapPost("/ratings", CreateMovieRatingAsync)
            .WithName(RatingEndpointsName.CreateMovieRating)
            .AddEndpointFilter<CreateMovieRatingValidationFilter>()
            .RequireAuthorization(Policy.UserOnly);
        ratings.MapPut("/ratings/{ratingId}", UpdateMovieRatingAsync)
            .WithName(RatingEndpointsName.UpdateMovieRating)
            .AddEndpointFilter<UpdateMovieRatingValidationFilter>()
            .RequireAuthorization(Policy.UserOnly);
        ratings.MapDelete("/ratings/{ratingId}", DeleteMovieRatingAsync)
            .WithName(RatingEndpointsName.DeleteMovieRating)
            .RequireAuthorization(Policy.UserOnly);
    }

    private static async Task<IResult> GetMovieRatingsAsync(HttpContext httpContext,
        [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromQuery] int pageNumber = Default.PageNumber, [FromQuery] int pageSize = Default.PageSize, CancellationToken cancellationToken = default)
    {
        var searchParameters = new SearchParameters(pageNumber, pageSize);
        var request = new GetMovieRatingsRequest(movieId, searchParameters);
        var result = await ratingService.GetMovieRatingsAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = ratingMapper.ToResponses(result.Value.Items);
        var links = GeneratePagedGetLinks(linkService, movieId, result.Value.HasPrevious, result.Value.HasNext, result.Value.CurrentPage, result.Value.PageSize);
        var endpointResult = new EndpointResult<ICollection<RatingResponse>>(response, links.ToList());

        var paginationMetadata = new PaginationMetadata(result.Value.TotalCount, result.Value.PageSize, result.Value.CurrentPage);
        httpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> GetMovieRatingByIdAsync(
        [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromRoute] int ratingId, CancellationToken cancellationToken = default)
    {
        var request = new GetMovieRatingByIdRequest(movieId,ratingId);
        var result = await ratingService.GetMovieRatingByIdAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = ratingMapper.ToResponse(result.Value);
        var links = GenerateGetLinks(linkService, movieId, response.Id);
        var endpointResult = new EndpointResult<RatingResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> CreateMovieRatingAsync(
        [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromBody] CreateMovieRatingRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new CreateMovieRatingRequest(movieId, requestBody.CreatorId, requestBody.SourPopcorns, requestBody.Comment);
        var result = await ratingService.CreateMovieRatingAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = ratingMapper.ToResponse(result.Value);
        var links = GenerateCreateLinks(linkService, movieId, response.Id);
        var endpointResult = new EndpointResult<RatingResponse>(response, links.ToList());

        return TypedResults.CreatedAtRoute(endpointResult, RatingEndpointsName.GetMovieRatingById, new { ratingId = response.Id });
    }

    private static async Task<IResult> UpdateMovieRatingAsync(HttpContext httpContext,
        [FromServices] ITokenService tokenService, [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromRoute] int ratingId, [FromBody] UpdateMovieRatingRequestBody requestBody, CancellationToken cancellationToken = default)
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

        var request = new UpdateMovieRatingRequest( userId.Value, roles, movieId, ratingId, requestBody.SourPopcorns, requestBody.Comment);
        var result = await ratingService.UpdateMovieRatingAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code switch {
                ErrorCode.NullValue => TypedResults.NotFound(result.Error.Message),
                ErrorCode.Forbidden => TypedResults.UnprocessableEntity(result.Error.Message),
                _ => TypedResults.Problem("Failed result error value is incorrect.")
            };

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = ratingMapper.ToResponse(result.Value);
        var links = GenerateUpdateLinks(linkService, movieId, response.Id);
        var endpointResult = new EndpointResult<RatingResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> DeleteMovieRatingAsync(HttpContext httpContext,
        [FromServices] ITokenService tokenService, [FromServices] IRatingService ratingService,
        [FromRoute] int movieId, [FromRoute] int ratingId, CancellationToken cancellationToken = default)
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

        var request = new DeleteMovieRatingRequest(userId.Value, roles, movieId, ratingId);
        var result = await ratingService.DeleteMovieRatingAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code switch
            {
                ErrorCode.NullValue => TypedResults.NotFound(result.Error.Message),
                ErrorCode.Forbidden => TypedResults.UnprocessableEntity(result.Error.Message),
                _ => TypedResults.Problem("Failed result error value is incorrect.")
            };

        return TypedResults.NoContent();
    }

    private static IEnumerable<Link> GeneratePagedGetLinks(ILinkService linkService, int movieId, bool hasPrevious, bool hasNext, int pageNumber, int pageSize)
    {
        if (hasPrevious)
            yield return linkService.Generate(RatingEndpointsName.GetMovieRatings, new { movieId, pageNumber = pageNumber - 1, pageSize }, "self", "GET");
        if (hasNext)
            yield return linkService.Generate(RatingEndpointsName.GetMovieRatings, new { movieId, pageNumber = pageNumber + 1, pageSize }, "self", "GET");
    }

    private static IEnumerable<Link> GenerateGetLinks(ILinkService linkService, int movieId, int ratingId)
    {
        yield return linkService.Generate(RatingEndpointsName.UpdateMovieRating, new { movieId, ratingId }, "self", "PUT");
        yield return linkService.Generate(RatingEndpointsName.DeleteMovieRating, new { movieId, ratingId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateCreateLinks(ILinkService linkService, int movieId, int ratingId)
    {
        yield return linkService.Generate(RatingEndpointsName.GetMovieRatingById, new { movieId, ratingId }, "self", "GET");
        yield return linkService.Generate(RatingEndpointsName.UpdateMovieRating, new { movieId, ratingId }, "self", "PUT");
        yield return linkService.Generate(RatingEndpointsName.DeleteMovieRating, new { movieId, ratingId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateUpdateLinks(ILinkService linkService, int movieId, int ratingId)
    {
        yield return linkService.Generate(RatingEndpointsName.GetMovieRatingById, new { movieId, ratingId }, "self", "GET");
        yield return linkService.Generate(RatingEndpointsName.DeleteMovieRating, new { movieId, ratingId }, "self", "DELETE");
    }
}
