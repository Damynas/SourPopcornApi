using Application.Users.Abstractions;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.Entities;
using MediatR;

namespace Application.Users.Services;

public class UserService(ISender sender) : IUserService
{
    public async Task<Result<PagedList<User>>> GetUsersAsync(GetUsersRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<User?>> GetUserByIdAsync(GetUserByIdRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<User?>> GetUserByUsernameAsync(GetUserByUsernameRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByUsernameQuery(request);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<Result<User>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<User?>> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
