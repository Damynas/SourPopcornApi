using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.Entities;

namespace Application.Users.Abstractions;

public interface IUserService
{
    Task<Result<PagedList<User>>> GetUsersAsync(GetUsersRequest request, CancellationToken cancellationToken = default);
    Task<Result<User?>> GetUserByIdAsync(GetUserByIdRequest request, CancellationToken cancellationToken = default);
    Task<Result<User?>> GetUserByUsernameAsync(GetUserByUsernameRequest request, CancellationToken cancellationToken = default);
    Task<Result<User>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<User?>> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken = default);
}
