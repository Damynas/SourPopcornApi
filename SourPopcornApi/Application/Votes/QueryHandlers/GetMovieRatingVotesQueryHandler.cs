using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Votes.Abstractions;
using Application.Votes.Queries;
using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Votes.Entities;

namespace Application.Votes.QueryHandlers;

public class GetMovieRatingVotesQueryHandler(IMovieRepository movieRepository, IVoteRepository voteRepository)
    : IQueryHandler<GetMovieRatingVotesQuery, PagedList<Vote>?>
{
    public async Task<Result<PagedList<Vote>?>> Handle(GetMovieRatingVotesQuery query, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(query.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<PagedList<Vote>?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == query.Request.RatingId);
        if (rating is null)
            return Result<PagedList<Vote>?>.Failure(null, Error.NullValue("Specified rating does not exist for specified movie."));

        var votePage = await voteRepository.GetRatingVotesAsync(rating.Id, query.Request.SearchParameters, cancellationToken);
        return Result<PagedList<Vote>?>.Success(votePage);
    }
}
