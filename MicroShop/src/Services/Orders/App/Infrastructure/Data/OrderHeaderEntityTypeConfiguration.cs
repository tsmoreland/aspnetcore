using MicroShop.Services.Orders.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.Orders.App.Infrastructure.Data;

public sealed class OrderHeaderEntityTypeConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(e => e.UserId).IsUnicode().HasMaxLength(450);
        builder.Property(e => e.Status).IsRequired();
        builder.Property(p => p.CouponCode).HasMaxLength(200);
        builder.Property(e => e.Discount);
        builder.Property(e => e.OrderTotal).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.OrderTime).IsRequired();
        builder.Property(e => e.EmailAddress).HasMaxLength(200).IsRequired();
        builder.Property(e => e.PaymentIntentId).HasMaxLength(500);
        builder.Property(e => e.StripeSessionId).HasMaxLength(500);

        builder.HasMany(e => e.Items)
            .WithOne(e => e.Header)
            .HasForeignKey(e => e.HeaderId);
    }
}
