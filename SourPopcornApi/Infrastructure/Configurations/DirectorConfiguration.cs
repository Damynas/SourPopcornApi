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
        builder.Property(d => d.Id).ValueGeneratedOnAdd();

        builder.Property(d => d.CreatedOn).IsRequired();
        builder.Property(d => d.ModifiedOn).IsRequired();
        builder.Property(d => d.IsDeleted).IsRequired();

        builder.Property(d => d.Name).IsRequired();
        builder.Property(d => d.Country).IsRequired();
        builder.Property(d => d.BornOn).IsRequired();

        builder
            .HasMany(d => d.Movies)
            .WithOne(m => m.Director)
            .HasForeignKey(m => m.DirectorId)
            .IsRequired();
    }
}
