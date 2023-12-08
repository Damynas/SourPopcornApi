using Application.Movies.Abstractions;
using Domain.Movies.DataTransferObjects.Responses;
using Domain.Movies.Entities;

namespace Infrastructure.Movies;

public class MovieMapper : IMovieMapper
{
    public MovieResponse ToResponse(Movie movie)
    {
        return new MovieResponse(
            movie.Id, movie.CreatedOn, movie.ModifiedOn,
            movie.DirectorId, movie.Title, movie.Description, movie.Country, movie.Language, movie.ReleasedOn, movie.Writers, movie.Actors);
    }

    public IEnumerable<MovieResponse> ToResponses(IEnumerable<Movie> movies)
    {
        return movies.Select(ToResponse).ToList();
    }
}
