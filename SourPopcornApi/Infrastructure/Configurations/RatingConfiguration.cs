using Domain.Ratings.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("ratings");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.CreatedOn).IsRequired();
        builder.Property(r => r.ModifiedOn).IsRequired();
        builder.Property(r => r.IsDeleted).IsRequired();

        builder.Property(v => v.MovieId).IsRequired();
        builder.Property(v => v.CreatorId).IsRequired();
        builder.Property(r => r.SourPopcorns).IsRequired();
        builder.Property(r => r.Comment).IsRequired();

        builder
            .HasMany(r => r.Votes)
            .WithOne(v => v.Rating)
            .HasForeignKey(v => v.RatingId)
            .IsRequired();
    }
}
