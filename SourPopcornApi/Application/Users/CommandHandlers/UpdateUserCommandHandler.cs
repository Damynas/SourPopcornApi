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
        var user = await userRepository.GetByIdAsync(command.Request.Id, cancellationToken);
        if (user is null)
            return Result<User?>.Failure(null, Error.NullValue);

        user.ModifiedOn = DateTime.UtcNow;
        user.DisplayName = command.Request.DisplayName;

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<User?>.Success(user);
    }
}
