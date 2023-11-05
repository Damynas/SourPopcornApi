using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.CreatedOn).IsRequired();
        builder.Property(u => u.ModifiedOn).IsRequired();
        builder.Property(u => u.IsDeleted).IsRequired();

        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.DisplayName).IsRequired();
        builder.Property(u => u.ForceLogin).IsRequired();

        builder
            .HasMany(r => r.Ratings)
            .WithOne(v => v.Creator)
            .HasForeignKey(v => v.CreatorId)
            .IsRequired();

        builder
            .HasMany(r => r.Votes)
            .WithOne(v => v.Creator)
            .HasForeignKey(v => v.CreatorId)
            .IsRequired();

        builder
            .Property(u => u.Roles)
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
