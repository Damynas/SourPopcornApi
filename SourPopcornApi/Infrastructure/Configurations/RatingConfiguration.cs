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
        builder.Property(r => r.Id)
            .IsRequired().HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.CreatedOn).IsRequired().HasColumnName("createdOn");
        builder.Property(r => r.ModifiedOn).IsRequired().HasColumnName("modifiedOn");
        builder.Property(r => r.IsDeleted).IsRequired().HasColumnName("isDeleted");

        builder.Property(r => r.MovieId).IsRequired().HasColumnName("movieId");
        builder.Property(r => r.CreatorId).IsRequired().HasColumnName("creatorId");
        builder.Property(r => r.SourPopcorns).IsRequired().HasColumnName("sourPopcorns");
        builder.Property(r => r.Comment).IsRequired().HasColumnName("comment");

        builder
            .HasMany(r => r.Votes)
            .WithOne(v => v.Rating)
            .HasForeignKey(v => v.RatingId)
            .IsRequired();
    }
}
