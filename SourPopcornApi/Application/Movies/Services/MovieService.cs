using Application.Movies.Abstractions;
using Application.Movies.Commands;
using Application.Movies.Queries;
using Domain.Movies.DataTransferObjects.Requests;
using Domain.Movies.Entities;
using Domain.Shared;
using Domain.Shared.Paging;
using MediatR;

namespace Application.Movies.Services;

public class MovieService(ISender sender) : IMovieService
{
    public async Task<Result<PagedList<Movie>>> GetMoviesAsync(GetMoviesRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetMoviesQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Movie?>> GetMovieByIdAsync(GetMovieByIdRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetMovieByIdQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Movie?>> CreateMovieAsync(CreateMovieRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateMovieCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<Movie?>> UpdateMovieAsync(UpdateMovieRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateMovieCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteMovieAsync(DeleteMovieRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteMovieCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
