using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Ratings.Abstractions;

public interface IRatingService
{
    Task<Result<PagedList<Rating>>> GetRatingsAsync(GetRatingsRequest request, CancellationToken cancellationToken = default);
    Task<Result<Rating?>> GetRatingByIdAsync(GetRatingByIdRequest request, CancellationToken cancellationToken = default);
    Task<Result<Rating?>> CreateRatingAsync(CreateRatingRequest request, CancellationToken cancellationToken = default);
    Task<Result<Rating?>> UpdateRatingAsync(UpdateRatingRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteRatingAsync(DeleteRatingRequest request, CancellationToken cancellationToken = default);
}
