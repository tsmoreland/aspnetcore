using MicroShop.Services.Orders.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.Orders.App.Infrastructure.Data;

public sealed class OrderDetailsEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetails>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<OrderDetails> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.HeaderId).IsRequired();
        builder
            .HasOne(e => e.Header)
            .WithMany()
            .HasForeignKey(e => e.HeaderId);

        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Price).IsRequired();
        builder.Property(e => e.Count).IsRequired().HasDefaultValue(0);
    }
}
