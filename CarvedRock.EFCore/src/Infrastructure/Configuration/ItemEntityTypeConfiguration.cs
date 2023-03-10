using CarvedRock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarvedRock.Infrastructure.Configuration;

public sealed class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(500);

        builder.Property(e => e.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(22,2)")
            .IsRequired();

        builder.Property(e => e.Weight)
            .HasColumnName("unit_weight")
            .HasColumnType("float(36)")
            .IsRequired();

        builder
            .HasMany(e => e.Orders)
            .WithMany(e => e.Items);
    }
}
