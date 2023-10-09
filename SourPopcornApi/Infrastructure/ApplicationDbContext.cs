using Application.Abstractions.Data;
using Domain.Directors.Entities;
using Domain.Movies.Entities;
using Domain.Ratings.Entities;
using Domain.Users.Entities;
using Domain.Votes.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }

    public DbSet<Director> Directors { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(InfrastructureAssemblyReference.Assembly);
    }
}
