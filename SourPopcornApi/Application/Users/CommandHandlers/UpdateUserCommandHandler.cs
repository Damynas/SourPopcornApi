using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Commands;
using Domain.Shared;
using Domain.Users.Entities;

namespace Application.Users.CommandHandlers;

public class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand, User?>
{
    public async Task<Result<User?>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Request.UserId, cancellationToken);
        if (user is null)
            return Result<User?>.Failure(null, Error.NullValue("Specified user does not exist."));

        user.ModifiedOn = DateTime.UtcNow;
        user.DisplayName = command.Request.DisplayName;
        if (command.Request.Roles is not null && command.Request.Roles.Count != 0)
        {
            user.Roles = [.. command.Request.Roles.Distinct().OrderByDescending(role => role)];
        }

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result<User?>.Success(user);
    }
}
