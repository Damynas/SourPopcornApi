using Application.Abstractions.Services;
using Application.Movies.Abstractions;
using Domain.Auth.Constants;
using Domain.Movies.DataTransferObjects.Requests;
using Domain.Movies.DataTransferObjects.Responses;
using Domain.Shared;
using Domain.Shared.Constants;
using Domain.Shared.Paging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Movies.Constants;
using Presentation.Movies.DataTransferObjects;
using Presentation.Movies.Filters;
using Presentation.Shared;
using System.Text.Json;

namespace Presentation.Movies.Endpoints;

public static class MovieEndpointsDefinition
{
    public static void AddMovieEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var movies = endpointRouteBuilder.MapGroup("/api").WithTags("Movies");
        movies.MapGet("movies", GetMoviesAsync)
            .WithName(MovieEndpointsName.GetMovies)
            .RequireAuthorization(Policy.User);
        movies.MapGet("/movies/{movieId}", GetMovieByIdAsync)
            .WithName(MovieEndpointsName.GetMovieById)
            .RequireAuthorization(Policy.User);
        movies.MapPost("/movies", CreateMovieAsync)
            .WithName(MovieEndpointsName.CreateMovie)
            .AddEndpointFilter<CreateMovieValidationFilter>()
            .RequireAuthorization(Policy.Moderator);
        movies.MapPut("/movies/{movieId}", UpdateMovieAsync)
            .WithName(MovieEndpointsName.UpdateMovie)
            .AddEndpointFilter<UpdateMovieValidationFilter>()
            .RequireAuthorization(Policy.Moderator);
        movies.MapDelete("/movies/{movieId}", DeleteMovieAsync)
            .WithName(MovieEndpointsName.DeleteMovie)
            .RequireAuthorization(Policy.Moderator);
    }

    private static async Task<IResult> GetMoviesAsync(HttpContext httpContext,
        [FromServices] IMovieService movieService, [FromServices] IMovieMapper movieMapper, [FromServices] ILinkService linkService,
        [FromQuery] int pageNumber = Default.PageNumber, [FromQuery] int pageSize = Default.PageSize, CancellationToken cancellationToken = default)
    {
        var searchParameters = new SearchParameters(pageNumber, pageSize);
        var request = new GetMoviesRequest(searchParameters);
        var result = await movieService.GetMoviesAsync(request, cancellationToken);

        var response = movieMapper.ToResponses(result.Value.Items);
        var links = GeneratePagedGetLinks(linkService, result.Value.HasPrevious, result.Value.HasNext, result.Value.CurrentPage, result.Value.PageSize);
        var endpointResult = new EndpointResult<ICollection<MovieResponse>>(response, links.ToList());

        var paginationMetadata = new PaginationMetadata(result.Value.TotalCount, result.Value.PageSize, result.Value.CurrentPage);
        httpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> GetMovieByIdAsync(
        [FromServices] IMovieService movieService, [FromServices] IMovieMapper movieMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, CancellationToken cancellationToken = default)
    {
        var request = new GetMovieByIdRequest(movieId);
        var result = await movieService.GetMovieByIdAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = movieMapper.ToResponse(result.Value);
        var links = GenerateGetLinks(linkService, movieId);
        var endpointResult = new EndpointResult<MovieResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> CreateMovieAsync(
        [FromServices] IMovieService movieService, [FromServices] IMovieMapper movieMapper, [FromServices] ILinkService linkService,
        [FromBody] CreateMovieRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new CreateMovieRequest(
            requestBody.DirectorId, requestBody.Description, requestBody.Country, requestBody.Language, requestBody.ReleasedOn, requestBody.Writers, requestBody.Actors);
        var result = await movieService.CreateMovieAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = movieMapper.ToResponse(result.Value);
        var links = GenerateCreateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<MovieResponse>(response, links.ToList());

        return TypedResults.CreatedAtRoute(endpointResult, MovieEndpointsName.GetMovieById, new { movieId = response.Id });
    }

    private static async Task<IResult> UpdateMovieAsync(
        [FromServices] IMovieService movieService, [FromServices] IMovieMapper movieMapper, [FromServices] ILinkService linkService,
        [FromRoute] int movieId, [FromBody] UpdateMovieRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new UpdateMovieRequest(
            movieId, requestBody.DirectorId, requestBody.Description, requestBody.Country, requestBody.Language, requestBody.ReleasedOn, requestBody.Writers, requestBody.Actors);
        var result = await movieService.UpdateMovieAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = movieMapper.ToResponse(result.Value);
        var links = GenerateUpdateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<MovieResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> DeleteMovieAsync(
        [FromServices] IMovieService movieService, [FromRoute] int movieId, CancellationToken cancellationToken = default)
    {
        var request = new DeleteMovieRequest(movieId);
        var result = await movieService.DeleteMovieAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        return TypedResults.NoContent();
    }

    private static IEnumerable<Link> GeneratePagedGetLinks(ILinkService linkService, bool hasPrevious, bool hasNext, int pageNumber, int pageSize)
    {
        if (hasPrevious)
            yield return linkService.Generate(MovieEndpointsName.GetMovies, new { pageNumber = pageNumber - 1, pageSize }, "self", "GET");
        if (hasNext)
            yield return linkService.Generate(MovieEndpointsName.GetMovies, new { pageNumber = pageNumber + 1, pageSize }, "self", "GET");
    }

    private static IEnumerable<Link> GenerateGetLinks(ILinkService linkService, int movieId)
    {
        yield return linkService.Generate(MovieEndpointsName.UpdateMovie, new { movieId }, "self", "PUT");
        yield return linkService.Generate(MovieEndpointsName.DeleteMovie, new { movieId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateCreateLinks(ILinkService linkService, int movieId)
    {
        yield return linkService.Generate(MovieEndpointsName.GetMovieById, new { movieId }, "self", "GET");
        yield return linkService.Generate(MovieEndpointsName.UpdateMovie, new { movieId }, "self", "PUT");
        yield return linkService.Generate(MovieEndpointsName.DeleteMovie, new { movieId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateUpdateLinks(ILinkService linkService, int movieId)
    {
        yield return linkService.Generate(MovieEndpointsName.GetMovieById, new { movieId }, "self", "GET");
        yield return linkService.Generate(MovieEndpointsName.DeleteMovie, new { movieId }, "self", "DELETE");
    }
}
