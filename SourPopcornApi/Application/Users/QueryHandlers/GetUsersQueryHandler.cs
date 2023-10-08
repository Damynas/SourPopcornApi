using Application.Abstractions.Messaging;
using Application.Users.Abstractions;
using Application.Users.Queries;
using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Users.Entities;

namespace Application.Users.QueryHandlers;

public class GetUsersQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUsersQuery, PagedList<User>>
{
    public async Task<Result<PagedList<User>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var userPage = await userRepository.GetAsync(query.Request.SearchParameters, cancellationToken);
        return Result<PagedList<User>>.Success(userPage);
    }
}
