using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Commands;
using Domain.Shared;

namespace Application.Users.CommandHandlers;

public class DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(Error.NullValue("Specified user does not exist."));

        user.ModifiedOn = DateTime.UtcNow;
        user.IsDeleted = true;

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result.Success();
    }
}
