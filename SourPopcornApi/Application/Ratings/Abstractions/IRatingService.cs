using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Ratings.Abstractions;

public interface IRatingService
{
    Task<Result<PagedList<Rating>?>> GetMovieRatingsAsync(GetMovieRatingsRequest request, CancellationToken cancellationToken = default);
    Task<Result<Rating?>> GetMovieRatingByIdAsync(GetMovieRatingByIdRequest request, CancellationToken cancellationToken = default);
    Task<Result<Rating?>> CreateMovieRatingAsync(CreateMovieRatingRequest request, CancellationToken cancellationToken = default);
    Task<Result<Rating?>> UpdateMovieRatingAsync(UpdateMovieRatingRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteMovieRatingAsync(DeleteMovieRatingRequest request, CancellationToken cancellationToken = default);
}
