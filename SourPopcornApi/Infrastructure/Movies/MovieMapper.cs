using Application.Directors.Abstractions;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Domain.Movies.DataTransferObjects.Responses;
using Domain.Movies.Entities;
using Domain.Shared.Utils;

namespace Infrastructure.Movies;

public class MovieMapper(IDirectorMapper directorMapper, IRatingMapper ratingMapper) : IMovieMapper
{
    private readonly IDirectorMapper _directorMapper = directorMapper;
    private readonly IRatingMapper _ratingMapper = ratingMapper;

    public MovieResponse ToResponse(Movie movie)
    {
        return new MovieResponse(
            movie.Id, movie.CreatedOn, movie.ModifiedOn,
            movie.Title, movie.PosterLink, movie.Description, movie.Country, movie.Language, movie.ReleasedOn, movie.Writers, movie.Actors,
            movie.Ratings.Count != 0 ? MathUtils.Round(movie.Ratings.Average(rating => rating.SourPopcorns), 1) : 0,
            movie.Director is not null ? _directorMapper.ToResponse(movie.Director) : null,
            movie.Ratings is not null ? _ratingMapper.ToResponses(movie.Ratings).ToList() : null);
    }

    public IEnumerable<MovieResponse> ToResponses(IEnumerable<Movie> movies)
    {
        return movies.Select(ToResponse).ToList();
    }
}
