using Domain.Auth.DataTransferObjects.Requests;
using Domain.Shared;
using Domain.Users.Entities;

namespace Application.Auth.Services;

public interface IAuthService
{
    Task<Result<User>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<User?>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
