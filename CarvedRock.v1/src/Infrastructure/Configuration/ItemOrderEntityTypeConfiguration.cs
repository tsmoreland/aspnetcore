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

public sealed class ItemOrderEntityTypeConfiguration : IEntityTypeConfiguration<ItemOrder>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ItemOrder> builder)
    {
        builder.ToTable("item_order");

        builder
            .HasOne(e => e.Item)
            .WithMany(e => e.ItemOrders)
            .HasForeignKey(e => e.ItemsId);

        builder
            .HasOne(e => e.Order)
            .WithMany(e => e.ItemOrders)
            .HasForeignKey(e => e.OrdersId);

        builder.HasKey(e => new { e.ItemsId, e.OrdersId });

        builder.Property(e => e.ItemsId)
            .HasColumnName("items_id");
        builder.Property(e => e.OrdersId)
            .HasColumnName("orders_id");
        builder.Property(e => e.OrderDate)
            .HasColumnName("order_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.Quantity)
            .HasColumnName("quantity");
    }
}
