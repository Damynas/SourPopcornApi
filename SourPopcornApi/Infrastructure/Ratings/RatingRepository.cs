using Application.Abstractions.Data;
using Application.Ratings.Abstractions;
using Domain.Ratings.Entities;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Ratings;

public class RatingRepository(IApplicationDbContext context) : Repository<Rating>(context), IRatingRepository
{
    public async Task<List<Rating>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Ratings
            .Where(x => x.MovieId == movieId && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Rating>> GetByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Ratings
            .Where(x => x.CreatorId == creatorId && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
