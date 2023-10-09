using Application.Directors.Abstractions;
using Application.Directors.Commands;
using Application.Directors.Queries;
using Domain.Directors.DataTransferObjects.Requests;
using Domain.Directors.Entities;
using Domain.Shared;
using Domain.Shared.Paging;
using MediatR;

namespace Application.Directors.Services;

public class DirectorService(ISender sender) : IDirectorService
{
    public async Task<Result<PagedList<Director>>> GetDirectorsAsync(GetDirectorsRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetDirectorsQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Director?>> GetDirectorByIdAsync(GetDirectorByIdRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetDirectorByIdQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<Director>> CreateDirectorAsync(CreateDirectorRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateDirectorCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<Director?>> UpdateDirectorAsync(UpdateDirectorRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateDirectorCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteDirectorAsync(DeleteDirectorRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteDirectorCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
