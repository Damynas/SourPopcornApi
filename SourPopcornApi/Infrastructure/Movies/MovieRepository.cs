using Application.Abstractions.Data;
using Application.Movies.Abstractions;
using Domain.Movies.Entities;
using Infrastructure.Abstractions;

namespace Infrastructure.Movies;

public class MovieRepository(IApplicationDbContext context) : Repository<Movie>(context), IMovieRepository { }
