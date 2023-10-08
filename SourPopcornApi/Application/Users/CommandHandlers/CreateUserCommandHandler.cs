using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Commands;
using Domain.Shared;
using Domain.Users.Constants;
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
            Roles = new List<string> { Role.User }
        };

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<User>.Success(user);
    }
}
