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
        builder.Property(v => v.Id).ValueGeneratedOnAdd();

        builder.Property(v => v.CreatedOn).IsRequired();
        builder.Property(v => v.ModifiedOn).IsRequired();
        builder.Property(v => v.IsDeleted).IsRequired();

        builder.Property(v => v.RatingId).IsRequired();
        builder.Property(v => v.CreatorId).IsRequired();
        builder.Property(v => v.IsPositive).IsRequired();
    }
}
