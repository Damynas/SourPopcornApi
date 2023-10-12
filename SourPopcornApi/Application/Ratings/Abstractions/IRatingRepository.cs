using Application.Abstractions.Data;
using Domain.Ratings.Entities;
using Domain.Shared.Paging;

namespace Application.Ratings.Abstractions;

public interface IRatingRepository : IRepository<Rating>
{
    Task<PagedList<Rating>> GetMovieRatingsAsync(int movieId, SearchParameters searchParameters, CancellationToken cancellationToken = default);

    Task<Rating?> GetMovieRatingByIdAsync(int movieId, int ratingId, CancellationToken cancellationToken = default);
}
