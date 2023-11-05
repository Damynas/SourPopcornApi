using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Auth.Commands;
using Application.Users.Abstractions;
using Domain.Shared;
using Domain.Users.Entities;

namespace Application.Auth.CommandHandlers;

public class LoginCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<LoginCommand, User?>
{
    public async Task<Result<User?>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(command.Request.Username, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.EnhancedVerify(command.Request.Password, user.PasswordHash))
            return Result<User?>.Failure(null, Error.NullValue("Username or password is incorrect."));

        user.ModifiedOn = DateTime.UtcNow;
        user.ForceLogin = false;

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<User?>.Success(user);
    }
}
