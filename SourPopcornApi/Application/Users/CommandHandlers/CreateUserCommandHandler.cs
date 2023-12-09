using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Commands;
using Domain.Auth.Constants;
using Domain.Shared;
using Domain.Users.Entities;

namespace Application.Users.CommandHandlers;

public class CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateUserCommand, User>
{
    public async Task<Result<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User(default, DateTime.UtcNow, DateTime.UtcNow)
        {
            Username = command.Request.Username,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(command.Request.Password),
            DisplayName = command.Request.DisplayName,
            Roles = command.Request.Roles is not null && command.Request.Roles.Count != 0 ? [.. command.Request.Roles.Distinct().OrderByDescending(role => role)] : [Role.User],
            ForceLogin = false
        };

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result<User>.Success(user);
    }
}
