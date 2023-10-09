using Application.Abstractions.Messaging;
using Application.Ratings.Abstractions;
using Application.Ratings.Queries;
using Domain.Ratings.Entities;
using Domain.Shared;

namespace Application.Ratings.QueryHandlers;

public class GetRatingByIdQueryHandler(IRatingRepository ratingRepository) : IQueryHandler<GetRatingByIdQuery, Rating?>
{
    public async Task<Result<Rating?>> Handle(GetRatingByIdQuery query, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(query.Request.Id, cancellationToken);
        if (rating is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified rating does not exist."));

        return Result<Rating?>.Success(rating);
    }
}
