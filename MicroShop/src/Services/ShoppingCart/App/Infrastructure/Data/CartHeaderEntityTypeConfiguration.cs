using MicroShop.Services.ShoppingCart.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.ShoppingCart.App.Infrastructure.Data;

public sealed class CartHeaderEntityTypeConfiguration : IEntityTypeConfiguration<CartHeader>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CartHeader> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(e => e.UserId).IsUnicode().HasMaxLength(450);
        builder.Property(p => p.CouponCode)
            .HasField("_couponCode")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasMaxLength(200);
        builder.Property(p => p.NormalizedCouponCode)
            .HasMaxLength(200);

        builder.Ignore(e => e.Discount);
        builder.Ignore(e => e.CartTotal);
    }
}
