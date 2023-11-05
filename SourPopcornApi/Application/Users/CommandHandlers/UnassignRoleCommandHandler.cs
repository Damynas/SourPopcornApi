using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Commands;
using Domain.Shared;

namespace Application.Users.CommandHandlers;

public class UnassignRoleCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UnassignRoleCommand>
{
    public async Task<Result> Handle(UnassignRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(Error.NullValue("Specified user does not exist."));

        if (!user.Roles.Contains(command.Request.Role))
            return Result.Failure(Error.Conflict("User does not have specified role."));

        user.ModifiedOn = DateTime.UtcNow;
        user.Roles.Remove(command.Request.Role);
        user.ForceLogin = true;

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
