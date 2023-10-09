using Domain.Movies.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("movies");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.CreatedOn).IsRequired();
        builder.Property(m => m.ModifiedOn).IsRequired();
        builder.Property(m => m.IsDeleted).IsRequired();

        builder.Property(m => m.DirectorId).IsRequired();
        builder.Property(m => m.Description).IsRequired();
        builder.Property(m => m.Country).IsRequired();
        builder.Property(m => m.Language).IsRequired();
        builder.Property(m => m.ReleasedOn).IsRequired();

        builder
            .HasMany(m => m.Ratings)
            .WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId)
            .IsRequired();

        builder
            .Property(m => m.Writers)
            .IsRequired()
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (collection1, collection2) => collection1 == null ? collection2 == null : (collection2 != null && collection1.SequenceEqual(collection2)),
                c => c == null ? 0 : c.GetHashCode())
            );

        builder
            .Property(m => m.Actors)
            .IsRequired()
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (collection1, collection2) => collection1 == null ? collection2 == null : (collection2 != null && collection1.SequenceEqual(collection2)),
                c => c == null ? 0 : c.GetHashCode())
            );
    }
}
