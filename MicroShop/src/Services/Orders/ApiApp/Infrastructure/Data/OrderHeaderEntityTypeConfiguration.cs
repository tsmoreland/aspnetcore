using MicroShop.Services.Orders.ApiApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.Orders.ApiApp.Infrastructure.Data;

public sealed class OrderHeaderEntityTypeConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(e => e.UserId).IsUnicode().HasMaxLength(450);
        builder.Property(p => p.CouponCode).HasMaxLength(200);
        builder.Property(e => e.Discount);
        builder.Property(e => e.OrderTotal).IsRequired();

        builder.OwnsMany(e => e.Items)
            .WithOwner(e => e.Header)
            .HasForeignKey(e => e.HeaderId);
    }
}
