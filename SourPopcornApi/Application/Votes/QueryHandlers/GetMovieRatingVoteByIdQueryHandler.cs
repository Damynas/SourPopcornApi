using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Votes.Queries;
using Domain.Shared;
using Domain.Votes.Entities;

namespace Application.Votes.QueryHandlers;

public class GetMovieRatingVoteByIdQueryHandler(IMovieRepository movieRepository) : IQueryHandler<GetMovieRatingVoteByIdQuery, Vote?>
{
    public async Task<Result<Vote?>> Handle(GetMovieRatingVoteByIdQuery query, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(query.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == query.Request.RatingId);
        if (rating is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified rating does not exist for specified movie."));

        var vote = rating.Votes.AsEnumerable().SingleOrDefault(x => x.Id == query.Request.VoteId);
        if (vote is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified vote does not exist for specified rating."));

        return Result<Vote?>.Success(vote);
    }
}
