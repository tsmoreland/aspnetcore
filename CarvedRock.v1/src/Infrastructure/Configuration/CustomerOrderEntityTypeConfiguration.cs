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

using CarvedRock.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarvedRock.Infrastructure.Configuration;

public sealed class CustomerOrderEntityTypeConfiguration : IEntityTypeConfiguration<CustomerOrder>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CustomerOrder> builder)
    {
        builder.HasNoKey();

        builder.Property(e => e.OrderId)
            .HasColumnName("orders_id");
        builder.Property(e => e.OrderName)
            .HasColumnName("orders_name");

        builder.Property(e => e.CustomerId)
            .HasColumnName("customers_id");
        builder.Property(e => e.CustomerName)
            .HasColumnName("customers_name");

        builder.Property(e => e.ItemId)
            .HasColumnName("item_id");
        builder.Property(e => e.ItemDescription)
            .HasColumnName("item_description");

        builder.Property(e => e.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(22,2)");
        builder.Property(e => e.PriceAfterVat)
            .HasColumnName("price_after_vat")
            .HasColumnType("decimal(22,2)");

        builder.ToView("customer_orders_view");
    }
}
