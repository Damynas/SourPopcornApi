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
        builder.Property(u => u.Id)
            .IsRequired().HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.CreatedOn).IsRequired().HasColumnName("createdOn");
        builder.Property(u => u.ModifiedOn).IsRequired().HasColumnName("modifiedOn");
        builder.Property(u => u.IsDeleted).IsRequired().HasColumnName("isDeleted");

        builder.Property(u => u.Username).IsRequired().HasColumnName("username");
        builder.Property(u => u.PasswordHash).IsRequired().HasColumnName("passwordHash");
        builder.Property(u => u.DisplayName).IsRequired().HasColumnName("displayName");
        builder.Property(u => u.ForceLogin).IsRequired().HasColumnName("forceLogin");

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
            .HasColumnName("roles")
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
