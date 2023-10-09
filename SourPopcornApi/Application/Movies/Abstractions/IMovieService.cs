using Domain.Movies.DataTransferObjects.Requests;
using Domain.Movies.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Movies.Abstractions;

public interface IMovieService
{
    Task<Result<PagedList<Movie>>> GetMoviesAsync(GetMoviesRequest request, CancellationToken cancellationToken = default);
    Task<Result<Movie?>> GetMovieByIdAsync(GetMovieByIdRequest request, CancellationToken cancellationToken = default);
    Task<Result<Movie?>> CreateMovieAsync(CreateMovieRequest request, CancellationToken cancellationToken = default);
    Task<Result<Movie?>> UpdateMovieAsync(UpdateMovieRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteMovieAsync(DeleteMovieRequest request, CancellationToken cancellationToken = default);
}
