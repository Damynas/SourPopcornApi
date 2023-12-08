using Application.Users.Abstractions;
using Domain.Users.DataTransferObjects.Responses;
using Domain.Users.Entities;

namespace Infrastructure.Users;

public class UserMapper : IUserMapper
{
    public UserResponse ToResponse(User user)
    {
        return new UserResponse(user.Id, user.CreatedOn, user.ModifiedOn, user.Username, user.DisplayName, user.Roles);
    }

    public IEnumerable<UserResponse> ToResponses(IEnumerable<User> users)
    {
        return users.Select(ToResponse).ToList();
    }
}
