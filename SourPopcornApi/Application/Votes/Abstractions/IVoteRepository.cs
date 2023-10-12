using Application.Abstractions.Data;
using Domain.Shared.Paging;
using Domain.Votes.Entities;

namespace Application.Votes.Abstractions;

public interface IVoteRepository : IRepository<Vote>
{
    Task<PagedList<Vote>> GetRatingVotesAsync(int ratingId, SearchParameters searchParameters, CancellationToken cancellationToken = default);

    Task<Vote?> GetRatingVoteByIdAsync(int ratingId, int voteId, CancellationToken cancellationToken = default);
}
