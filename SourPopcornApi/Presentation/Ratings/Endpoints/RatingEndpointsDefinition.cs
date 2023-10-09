using Application.Abstractions.Services;
using Application.Ratings.Abstractions;
using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.DataTransferObjects.Responses;
using Domain.Shared;
using Domain.Shared.Constants;
using Domain.Shared.Paging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Ratings.DataTransferObjects;
using Presentation.Ratings.Filters;
using Presentation.Shared;
using System.Text.Json;

namespace Presentation.Ratings.Endpoints;

public static class RatingEndpointsDefinition
{
    private const string GetRatings = "GetRatings";
    private const string GetRatingById = "GetRatingById";
    private const string CreateRating = "CreateRating";
    private const string UpdateRating = "UpdateRating";
    private const string DeleteRating = "DeleteRating";

    public static void AddRatingEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ratings = endpointRouteBuilder.MapGroup("/api").WithTags("Ratings");
        ratings.MapGet("/ratings", GetRatingsAsync)
            .WithName(GetRatings);
        ratings.MapGet("/ratings/{ratingId}", GetRatingByIdAsync)
            .WithName(GetRatingById);
        ratings.MapPost("/ratings/new", CreateRatingAsync)
            .WithName(CreateRating)
            .AddEndpointFilter<CreateRatingValidationFilter>();
        ratings.MapPut("/ratings/{ratingId}", UpdateRatingAsync)
            .WithName(UpdateRating)
            .AddEndpointFilter<UpdateRatingValidationFilter>();
        ratings.MapDelete("/ratings/{ratingId}", DeleteRatingAsync)
            .WithName(DeleteRating);
    }

    private static async Task<IResult> GetRatingsAsync(HttpContext httpContext,
        [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromQuery] int pageNumber = Default.PageNumber, [FromQuery] int pageSize = Default.PageSize, CancellationToken cancellationToken = default)
    {
        var searchParameters = new SearchParameters(pageNumber, pageSize);
        var request = new GetRatingsRequest(searchParameters);
        var result = await ratingService.GetRatingsAsync(request, cancellationToken);

        var response = ratingMapper.ToResponses(result.Value.Items);
        var links = GeneratePagedGetLinks(linkService, result.Value.HasPrevious, result.Value.HasNext, result.Value.CurrentPage, result.Value.PageSize);
        var endpointResult = new EndpointResult<ICollection<RatingResponse>>(response, links.ToList());

        var paginationMetadata = new PaginationMetadata(result.Value.TotalCount, result.Value.PageSize, result.Value.CurrentPage);
        httpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> GetRatingByIdAsync(
        [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromRoute] int ratingId, CancellationToken cancellationToken = default)
    {
        var request = new GetRatingByIdRequest(ratingId);
        var result = await ratingService.GetRatingByIdAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = ratingMapper.ToResponse(result.Value);
        var links = GenerateGetLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<RatingResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> CreateRatingAsync(
        [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromBody] CreateRatingRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new CreateRatingRequest(requestBody.MovieId, requestBody.CreatorId, requestBody.SourPopcorns, requestBody.Comment);
        var result = await ratingService.CreateRatingAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = ratingMapper.ToResponse(result.Value);
        var links = GenerateCreateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<RatingResponse>(response, links.ToList());

        return TypedResults.CreatedAtRoute(endpointResult, GetRatingById, new { ratingId = response.Id });
    }

    private static async Task<IResult> UpdateRatingAsync(
        [FromServices] IRatingService ratingService, [FromServices] IRatingMapper ratingMapper, [FromServices] ILinkService linkService,
        [FromRoute] int ratingId, [FromBody] UpdateRatingRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new UpdateRatingRequest(ratingId, requestBody.SourPopcorns, requestBody.Comment);
        var result = await ratingService.UpdateRatingAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = ratingMapper.ToResponse(result.Value);
        var links = GenerateUpdateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<RatingResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> DeleteRatingAsync(
        [FromServices] IRatingService ratingService, [FromRoute] int ratingId, CancellationToken cancellationToken = default)
    {
        var request = new DeleteRatingRequest(ratingId);
        await ratingService.DeleteRatingAsync(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static IEnumerable<Link> GeneratePagedGetLinks(ILinkService linkService, bool hasPrevious, bool hasNext, int pageNumber, int pageSize)
    {
        if (hasPrevious)
            yield return linkService.Generate(GetRatings, new { pageNumber = pageNumber - 1, pageSize }, "self", "GET");
        if (hasNext)
            yield return linkService.Generate(GetRatings, new { pageNumber = pageNumber + 1, pageSize }, "self", "GET");
    }

    private static IEnumerable<Link> GenerateGetLinks(ILinkService linkService, int ratingId)
    {
        yield return linkService.Generate(UpdateRating, new { ratingId }, "self", "PUT");
        yield return linkService.Generate(DeleteRating, new { ratingId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateCreateLinks(ILinkService linkService, int ratingId)
    {
        yield return linkService.Generate(GetRatingById, new { ratingId }, "self", "GET");
        yield return linkService.Generate(UpdateRating, new { ratingId }, "self", "PUT");
        yield return linkService.Generate(DeleteRating, new { ratingId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateUpdateLinks(ILinkService linkService, int ratingId)
    {
        yield return linkService.Generate(GetRatingById, new { ratingId }, "self", "GET");
        yield return linkService.Generate(DeleteRating, new { ratingId }, "self", "DELETE");
    }
}
