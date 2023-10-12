using Application.Abstractions.Data;
using Application.Votes.Abstractions;
using Domain.Shared.Paging;
using Domain.Votes.Entities;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Votes;

public class VoteRepository(IApplicationDbContext context) : Repository<Vote>(context), IVoteRepository
{
    public async Task<PagedList<Vote>> GetRatingVotesAsync(int ratingId, SearchParameters searchParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Votes
           .AsQueryable()
           .Where(x => x.RatingId == ratingId && !x.IsDeleted)
           .OrderBy(x => x.CreatedOn);

        return await PagedList<Vote>.CreateAsync(query, searchParameters.PageNumber, searchParameters.PageSize, cancellationToken);
    }

    public async Task<Vote?> GetRatingVoteByIdAsync(int ratingId, int voteId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Votes
            .Where(x => x.RatingId == ratingId && x.Id == voteId && !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
