using Application.Abstractions.Data;
using Application.Movies.Abstractions;
using Domain.Movies.Entities;
using Domain.Users.Entities;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Movies;

public class MovieRepository(IApplicationDbContext context) : Repository<Movie>(context), IMovieRepository
{
    public async Task<List<Movie>> GetByDirectorIdAsync(int directorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Movies
            .Where(x => x.DirectorId == directorId && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
