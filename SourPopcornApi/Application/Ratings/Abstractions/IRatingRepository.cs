using Application.Abstractions.Data;
using Domain.Ratings.Entities;

namespace Application.Ratings.Abstractions;

public interface IRatingRepository : IRepository<Rating>
{
    Task<List<Rating>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default);

    Task<List<Rating>> GetByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default);
}
