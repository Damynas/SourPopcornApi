﻿using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Queries;
using Domain.Shared;
using Domain.Users.Entities;

namespace Application.Users.QueryHandlers;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, User?>
{
    public async Task<Result<User?>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(query.Request.UserId, cancellationToken);
        if (user is null)
            return Result<User?>.Failure(null, Error.NullValue("Specified user does not exist."));

        return Result<User?>.Success(user);
    }
}
