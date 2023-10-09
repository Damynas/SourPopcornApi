using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Votes.DataTransferObjects.Requests;
using Domain.Votes.Entities;

namespace Application.Votes.Abstractions;

public interface IVoteService
{
    Task<Result<PagedList<Vote>>> GetVotesAsync(GetVotesRequest request, CancellationToken cancellationToken = default);
    Task<Result<Vote?>> GetVoteByIdAsync(GetVoteByIdRequest request, CancellationToken cancellationToken = default);
    Task<Result<Vote?>> CreateVoteAsync(CreateVoteRequest request, CancellationToken cancellationToken = default);
    Task<Result<Vote?>> UpdateVoteAsync(UpdateVoteRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteVoteAsync(DeleteVoteRequest request, CancellationToken cancellationToken = default);
}
