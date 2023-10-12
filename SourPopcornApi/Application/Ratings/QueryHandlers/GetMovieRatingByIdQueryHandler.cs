using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Ratings.Queries;
using Domain.Ratings.Entities;
using Domain.Shared;

namespace Application.Ratings.QueryHandlers;

public class GetMovieRatingByIdQueryHandler(IMovieRepository movieRepository) : IQueryHandler<GetMovieRatingByIdQuery, Rating?>
{
    public async Task<Result<Rating?>> Handle(GetMovieRatingByIdQuery query, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(query.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == query.Request.RatingId);
        if (rating is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified rating does not exist for specified movie."));

        return Result<Rating?>.Success(rating);
    }
}
