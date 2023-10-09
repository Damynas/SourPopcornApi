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
    public async Task<Result<PagedList<Rating>>> GetRatingsAsync(GetRatingsRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetRatingsQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Rating?>> GetRatingByIdAsync(GetRatingByIdRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetRatingByIdQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Rating?>> CreateRatingAsync(CreateRatingRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateRatingCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<Rating?>> UpdateRatingAsync(UpdateRatingRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateRatingCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteRatingAsync(DeleteRatingRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteRatingCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
