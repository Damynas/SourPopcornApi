using Application.Ratings.Abstractions;
using Application.Ratings.Commands;
using Application.Ratings.Queries;
using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.Entities;
using Domain.Shared;
using Domain.Shared.Paging;
using MediatR;

namespace Application.Ratings.Services;

public class RatingService(ISender sender) : IRatingService
{
    public async Task<Result<PagedList<Rating>?>> GetMovieRatingsAsync(GetMovieRatingsRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetMovieRatingsQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Rating?>> GetMovieRatingByIdAsync(GetMovieRatingByIdRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetMovieRatingByIdQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Rating?>> CreateMovieRatingAsync(CreateMovieRatingRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateMovieRatingCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<Rating?>> UpdateMovieRatingAsync(UpdateMovieRatingRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateMovieRatingCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteMovieRatingAsync(DeleteMovieRatingRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteMovieRatingCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
