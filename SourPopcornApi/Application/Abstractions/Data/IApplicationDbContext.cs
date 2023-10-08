using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
