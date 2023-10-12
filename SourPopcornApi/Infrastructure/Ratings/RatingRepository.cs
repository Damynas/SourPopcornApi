using Application.Abstractions.Data;
using Application.Ratings.Abstractions;
using Domain.Ratings.Entities;
using Domain.Shared.Paging;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Ratings;

public class RatingRepository(IApplicationDbContext context) : Repository<Rating>(context), IRatingRepository
{
    public async Task<PagedList<Rating>> GetMovieRatingsAsync(int movieId, SearchParameters searchParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Ratings
           .AsQueryable()
           .Where(x => x.MovieId == movieId && !x.IsDeleted)
           .OrderBy(x => x.CreatedOn);

        return await PagedList<Rating>.CreateAsync(query, searchParameters.PageNumber, searchParameters.PageSize, cancellationToken);
    }

    public async Task<Rating?> GetMovieRatingByIdAsync(int movieId, int ratingId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Ratings
            .Where(x => x.MovieId == movieId && x.Id == ratingId && !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
