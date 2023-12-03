using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Auth.Commands;
using Application.Users.Abstractions;
using Domain.Shared;

namespace Application.Auth.CommandHandlers;

public class LogoutCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(Error.NullValue("User does not exist."));

        user.ModifiedOn = DateTime.UtcNow;
        user.ForceLogin = true;

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
