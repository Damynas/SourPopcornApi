using Domain.Directors.Entities;
using Domain.Movies.Entities;
using Domain.Ratings.Entities;
using Domain.Users.Entities;
using Domain.Votes.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<Director> Directors { get; set; }

    DbSet<Movie> Movies { get; set; }

    DbSet<Rating> Ratings { get; set; }

    DbSet<Vote> Votes { get; set; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
