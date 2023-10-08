using Application.Abstractions.Data;
using Application.Users.Abstractions;
using Domain.Users.Entities;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users;

public class UserRepository(IApplicationDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .SingleOrDefaultAsync(x => x.Username == username && !x.IsDeleted, cancellationToken);
    }
}
