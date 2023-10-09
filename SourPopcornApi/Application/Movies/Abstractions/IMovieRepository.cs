using Application.Abstractions.Data;
using Domain.Movies.Entities;

namespace Application.Movies.Abstractions;

public interface IMovieRepository : IRepository<Movie>
{
    Task<List<Movie>> GetByDirectorIdAsync(int directorId, CancellationToken cancellationToken = default);
}
