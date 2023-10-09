using Application.Abstractions.Services;
using Application.Votes.Abstractions;
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
using Presentation.Votes.DataTransferObjects;
using System.Text.Json;

namespace Presentation.Votes.Endpoints;

public static class VoteEndpointsDefinition
{
    private const string GetVotes = "GetVotes";
    private const string GetVoteById = "GetVoteById";
    private const string CreateVote = "CreateVote";
    private const string UpdateVote = "UpdateVote";
    private const string DeleteVote = "DeleteVote";

    public static void AddVoteEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var votes = endpointRouteBuilder.MapGroup("/api").WithTags("Votes");
        votes.MapGet("/votes", GetVotesAsync)
            .WithName(GetVotes);
        votes.MapGet("/votes/{voteId}", GetVoteByIdAsync)
            .WithName(GetVoteById);
        votes.MapPost("/votes/new", CreateVoteAsync)
            .WithName(CreateVote);
        votes.MapPut("/votes/{voteId}", UpdateVoteAsync)
            .WithName(UpdateVote);
        votes.MapDelete("/votes/{voteId}", DeleteVoteAsync)
            .WithName(DeleteVote);
    }

    private static async Task<IResult> GetVotesAsync(HttpContext httpContext,
        [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromQuery] int pageNumber = Default.PageNumber, [FromQuery] int pageSize = Default.PageSize, CancellationToken cancellationToken = default)
    {
        var searchParameters = new SearchParameters(pageNumber, pageSize);
        var request = new GetVotesRequest(searchParameters);
        var result = await voteService.GetVotesAsync(request, cancellationToken);

        var response = voteMapper.ToResponses(result.Value.Items);
        var links = GeneratePagedGetLinks(linkService, result.Value.HasPrevious, result.Value.HasNext, result.Value.CurrentPage, result.Value.PageSize);
        var endpointResult = new EndpointResult<ICollection<VoteResponse>>(response, links.ToList());

        var paginationMetadata = new PaginationMetadata(result.Value.TotalCount, result.Value.PageSize, result.Value.CurrentPage);
        httpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> GetVoteByIdAsync(
        [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromRoute] int voteId, CancellationToken cancellationToken = default)
    {
        var request = new GetVoteByIdRequest(voteId);
        var result = await voteService.GetVoteByIdAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = voteMapper.ToResponse(result.Value);
        var links = GenerateGetLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<VoteResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> CreateVoteAsync(
        [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromBody] CreateVoteRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new CreateVoteRequest(requestBody.RatingId, requestBody.CreatorId, requestBody.IsPositive);
        var result = await voteService.CreateVoteAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = voteMapper.ToResponse(result.Value);
        var links = GenerateCreateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<VoteResponse>(response, links.ToList());

        return TypedResults.CreatedAtRoute(endpointResult, GetVoteById, new { voteId = response.Id });
    }

    private static async Task<IResult> UpdateVoteAsync(
        [FromServices] IVoteService voteService, [FromServices] IVoteMapper voteMapper, [FromServices] ILinkService linkService,
        [FromRoute] int voteId, [FromBody] UpdateVoteRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var request = new UpdateVoteRequest(voteId, requestBody.IsPositive);
        var result = await voteService.UpdateVoteAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code == ErrorCode.NullValue ? TypedResults.NotFound(result.Error.Message) : TypedResults.Problem("Failed result error value is incorrect.");

        if (result.Value is null)
            return TypedResults.Problem("Successfull result value cannot be null.");

        var response = voteMapper.ToResponse(result.Value);
        var links = GenerateUpdateLinks(linkService, response.Id);
        var endpointResult = new EndpointResult<VoteResponse>(response, links.ToList());

        return TypedResults.Ok(endpointResult);
    }

    private static async Task<IResult> DeleteVoteAsync(
        [FromServices] IVoteService voteService, [FromRoute] int voteId, CancellationToken cancellationToken = default)
    {
        var request = new DeleteVoteRequest(voteId);
        await voteService.DeleteVoteAsync(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static IEnumerable<Link> GeneratePagedGetLinks(ILinkService linkService, bool hasPrevious, bool hasNext, int pageNumber, int pageSize)
    {
        if (hasPrevious)
            yield return linkService.Generate(GetVotes, new { pageNumber = pageNumber - 1, pageSize }, "self", "GET");
        if (hasNext)
            yield return linkService.Generate(GetVotes, new { pageNumber = pageNumber + 1, pageSize }, "self", "GET");
    }

    private static IEnumerable<Link> GenerateGetLinks(ILinkService linkService, int voteId)
    {
        yield return linkService.Generate(UpdateVote, new { voteId }, "self", "PUT");
        yield return linkService.Generate(DeleteVote, new { voteId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateCreateLinks(ILinkService linkService, int voteId)
    {
        yield return linkService.Generate(GetVoteById, new { voteId }, "self", "GET");
        yield return linkService.Generate(UpdateVote, new { voteId }, "self", "PUT");
        yield return linkService.Generate(DeleteVote, new { voteId }, "self", "DELETE");
    }

    private static IEnumerable<Link> GenerateUpdateLinks(ILinkService linkService, int voteId)
    {
        yield return linkService.Generate(GetVoteById, new { voteId }, "self", "GET");
        yield return linkService.Generate(DeleteVote, new { voteId }, "self", "DELETE");
    }
}
