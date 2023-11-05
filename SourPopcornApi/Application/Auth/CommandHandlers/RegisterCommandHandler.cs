using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Auth.Commands;
using Application.Users.Abstractions;
using Domain.Auth.Constants;
using Domain.Shared;
using Domain.Users.Entities;

namespace Application.Auth.CommandHandlers;
public class RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<RegisterCommand, User>
{
    public async Task<Result<User>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var user = new User(default, DateTime.UtcNow, DateTime.UtcNow)
        {
            Username = command.Request.Username,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(command.Request.Password),
            DisplayName = command.Request.DisplayName,
            Roles = [Role.User],
            ForceLogin = false
        };

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<User>.Success(user);
    }
}
