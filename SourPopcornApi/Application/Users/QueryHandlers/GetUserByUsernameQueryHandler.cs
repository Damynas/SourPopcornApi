using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Queries;
using Domain.Shared;
using Domain.Users.Entities;

namespace Application.Users.QueryHandlers;

public class GetUserByUsernameQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByUsernameQuery, User?>
{
    public async Task<Result<User?>> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(query.Request.Username, cancellationToken);
        if (user is null)
            return Result<User?>.Failure(null, Error.NullValue);

        return Result<User?>.Success(user);
    }
}
