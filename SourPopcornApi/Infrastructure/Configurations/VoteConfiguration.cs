using Domain.Votes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("votes");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .IsRequired().HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(v => v.CreatedOn).IsRequired().HasColumnName("createdOn");
        builder.Property(v => v.ModifiedOn).IsRequired().HasColumnName("modifiedOn");
        builder.Property(v => v.IsDeleted).IsRequired().HasColumnName("isDeleted");

        builder.Property(v => v.RatingId).IsRequired().HasColumnName("ratingId");
        builder.Property(v => v.CreatorId).IsRequired().HasColumnName("creatorId");
        builder.Property(v => v.IsPositive).IsRequired().HasColumnName("isPositive");
    }
}
