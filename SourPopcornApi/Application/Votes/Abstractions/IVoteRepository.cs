using Application.Abstractions.Data;
using Domain.Votes.Entities;

namespace Application.Votes.Abstractions;

public interface IVoteRepository : IRepository<Vote>
{
    Task<List<Vote>> GetByRatingIdAsync(int ratingId, CancellationToken cancellationToken = default);

    Task<List<Vote>> GetByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default);
}
