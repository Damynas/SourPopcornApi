using Application.Abstractions.Data;
using Domain.Users.Entities;

namespace Application.Users.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
