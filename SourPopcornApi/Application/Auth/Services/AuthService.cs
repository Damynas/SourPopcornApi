using Application.Auth.Commands;
using Domain.Auth.DataTransferObjects.Requests;
using Domain.Shared;
using Domain.Users.Entities;
using MediatR;

namespace Application.Auth.Services;

public class AuthService(ISender sender) : IAuthService
{
    public async Task<Result<User>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var command = new RegisterCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result<User?>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var command = new LoginCommand(request);
        return await sender.Send(command, cancellationToken);
    }

    public async Task<Result> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        var command = new LogoutCommand(request);
        return await sender.Send(command, cancellationToken);
    }
}
