using Application.Abstractions.Data;
using Domain.Movies.Entities;

namespace Application.Movies.Abstractions;

public interface IMovieRepository : IRepository<Movie> { }
