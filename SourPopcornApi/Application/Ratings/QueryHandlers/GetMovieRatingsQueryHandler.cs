using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Application.Ratings.Queries;
using Domain.Ratings.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Ratings.QueryHandlers;

public class GetMovieRatingsQueryHandler(IMovieRepository movieRepository, IRatingRepository ratingRepository) : IQueryHandler<GetMovieRatingsQuery, PagedList<Rating>?>
{
    public async Task<Result<PagedList<Rating>?>> Handle(GetMovieRatingsQuery query, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(query.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<PagedList<Rating>?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var ratingPage = await ratingRepository.GetMovieRatingsAsync(movie.Id, query.Request.SearchParameters, cancellationToken);
        return Result<PagedList<Rating>?>.Success(ratingPage);
    }
}
