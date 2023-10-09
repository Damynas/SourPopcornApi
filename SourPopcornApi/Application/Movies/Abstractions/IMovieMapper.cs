using Application.Abstractions.Data;
using Domain.Movies.DataTransferObjects.Responses;
using Domain.Movies.Entities;

namespace Application.Movies.Abstractions;

public interface IMovieMapper : IMapper<Movie, MovieResponse> { }
