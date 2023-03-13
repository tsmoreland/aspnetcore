//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using CarvedRock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarvedRock.Infrastructure.Configuration;

public sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasIndex(e => e.Name);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(50);

        builder.Property(e => e.CustomerId)
            .IsRequired()
            .HasColumnName("customer_id");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder
            .HasOne(e => e.Customer)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.CustomerId);

        builder
            .HasMany(e => e.Items)
            .WithMany(e => e.Orders)
            .UsingEntity<ItemOrder>(
                static rightBuilder =>
                    rightBuilder
                        .HasOne(e => e.Item)
                        .WithMany()
                        .HasForeignKey(e => e.ItemsId),
                static leftBuilder =>
                    leftBuilder
                        .HasOne(e => e.Order)
                        .WithMany()
                        .HasForeignKey(e => e.OrdersId),
                static joinBuilder =>
                {
                    joinBuilder.ToTable("item_order");
                    joinBuilder.HasKey(e => new { e.ItemsId, e.OrdersId });
                    joinBuilder.Property(e => e.ItemsId)
                        .HasColumnName("items_id");
                    joinBuilder.Property(e => e.OrdersId)
                        .HasColumnName("orders_id");
                    joinBuilder.Property(e => e.OrderDate)
                        .HasColumnName("order_date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
                    joinBuilder.Property(e => e.Quantity)
                        .HasColumnName("quantity");
                });
    }
}
