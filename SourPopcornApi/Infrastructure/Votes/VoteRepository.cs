using Application.Abstractions.Data;
using Application.Votes.Abstractions;
using Domain.Votes.Entities;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Votes;

public class VoteRepository(IApplicationDbContext context) : Repository<Vote>(context), IVoteRepository
{
    public async Task<List<Vote>> GetByRatingIdAsync(int ratingId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Votes
            .Where(x => x.RatingId == ratingId && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Vote>> GetByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Votes
            .Where(x => x.CreatorId == creatorId && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
