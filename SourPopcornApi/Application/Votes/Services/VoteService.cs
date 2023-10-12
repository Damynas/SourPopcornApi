using Application.Votes.Abstractions;
using Application.Votes.Commands;
using Application.Votes.Queries;
using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Votes.DataTransferObjects.Requests;
using Domain.Votes.Entities;
using MediatR;

namespace Application.Votes.Services;

public class VoteService(ISender sender) : IVoteService
{
    public async Task<Result<PagedList<Vote>?>> GetMovieRatingVotesAsync(GetMovieRatingVotesRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetMovieRatingVotesQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Vote?>> GetMovieRatingVoteByIdAsync(GetMovieRatingVoteByIdRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetMovieRatingVoteByIdQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Vote?>> CreateMovieRatingVoteAsync(CreateMovieRatingVoteRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateMovieRatingVoteCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<Vote?>> UpdateMovieRatingVoteAsync(UpdateMovieRatingVoteRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateMovieRatingVoteCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteMovieRatingVoteAsync(DeleteMovieRatingVoteRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteMovieRatingVoteCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
