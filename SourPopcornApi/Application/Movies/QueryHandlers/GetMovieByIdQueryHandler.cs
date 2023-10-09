using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Movies.Queries;
using Domain.Movies.Entities;
using Domain.Shared;

namespace Application.Movies.QueryHandlers;

public class GetMovieByIdQueryHandler(IMovieRepository movieRepository) : IQueryHandler<GetMovieByIdQuery, Movie?>
{
    public async Task<Result<Movie?>> Handle(GetMovieByIdQuery query, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(query.Request.Id, cancellationToken);
        if (movie is null)
            return Result<Movie?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        return Result<Movie?>.Success(movie);
    }
}
