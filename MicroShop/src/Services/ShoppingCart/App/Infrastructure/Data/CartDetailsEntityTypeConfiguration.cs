using MicroShop.Services.ShoppingCart.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.ShoppingCart.App.Infrastructure.Data;

public sealed class CartDetailsEntityTypeConfiguration : IEntityTypeConfiguration<CartDetails>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CartDetails> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.HeaderId).IsRequired();
        builder
            .HasOne(e => e.Header)
            .WithMany()
            .HasForeignKey(e => e.HeaderId);

        builder.Property(e => e.ProductId).IsRequired();
        builder.Ignore(e => e.Product);

        builder.Property(e => e.Count).IsRequired().HasDefaultValue(0);

    }
}
