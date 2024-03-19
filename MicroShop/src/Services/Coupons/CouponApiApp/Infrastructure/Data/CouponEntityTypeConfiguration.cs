using MicroShop.Services.Coupons.CouponApiApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.Coupons.CouponApiApp.Infrastructure.Data;

public sealed class CouponEntityTypeConfiguration : IEntityTypeConfiguration<Coupon>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.NormalizedCode);

        builder.Property(p => p.Code)
            .HasField("_code")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(p => p.NormalizedCode)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.DiscountAmount).IsRequired();
    }
}
