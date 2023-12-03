using Domain.Directors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.ToTable("directors");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .IsRequired().HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.CreatedOn).IsRequired().HasColumnName("createdOn");
        builder.Property(d => d.ModifiedOn).IsRequired().HasColumnName("modifiedOn");
        builder.Property(d => d.IsDeleted).IsRequired().HasColumnName("isDeleted");

        builder.Property(d => d.Name).IsRequired().HasColumnName("name");
        builder.Property(d => d.Country).IsRequired().HasColumnName("country");
        builder.Property(d => d.BornOn).IsRequired().HasColumnName("bornOn");

        builder
            .HasMany(d => d.Movies)
            .WithOne(m => m.Director)
            .HasForeignKey(m => m.DirectorId)
            .IsRequired();
    }
}
