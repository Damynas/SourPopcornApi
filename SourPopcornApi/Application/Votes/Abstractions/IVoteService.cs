using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Votes.DataTransferObjects.Requests;
using Domain.Votes.Entities;

namespace Application.Votes.Abstractions;

public interface IVoteService
{
    Task<Result<PagedList<Vote>?>> GetMovieRatingVotesAsync(GetMovieRatingVotesRequest request, CancellationToken cancellationToken = default);
    Task<Result<Vote?>> GetMovieRatingVoteByIdAsync(GetMovieRatingVoteByIdRequest request, CancellationToken cancellationToken = default);
    Task<Result<Vote?>> CreateMovieRatingVoteAsync(CreateMovieRatingVoteRequest request, CancellationToken cancellationToken = default);
    Task<Result<Vote?>> UpdateMovieRatingVoteAsync(UpdateMovieRatingVoteRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteMovieRatingVoteAsync(DeleteMovieRatingVoteRequest request, CancellationToken cancellationToken = default);
}
