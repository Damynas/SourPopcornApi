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
    public async Task<Result<PagedList<Vote>>> GetVotesAsync(GetVotesRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetVotesQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Vote?>> GetVoteByIdAsync(GetVoteByIdRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetVoteByIdQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Vote?>> CreateVoteAsync(CreateVoteRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateVoteCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<Vote?>> UpdateVoteAsync(UpdateVoteRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateVoteCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteVoteAsync(DeleteVoteRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteVoteCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
