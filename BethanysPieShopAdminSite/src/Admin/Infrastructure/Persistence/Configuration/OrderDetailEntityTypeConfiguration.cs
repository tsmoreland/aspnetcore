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

using BethanysPieShop.Admin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Configuration;

public sealed class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
            .HasColumnName("amount")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasPrecision(18,2)
            .IsRequired();

        builder.Property(e => e.OrderId).HasColumnName("order_id")
            .IsRequired();
        builder.HasOne(e => e.Order)
            .WithMany(e => e.OrderDetails)
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.PieId).HasColumnName("pie_id")
            .IsRequired();
        builder.HasOne(e => e.Pie);

        builder.Property(e => e.ConcurrencyToken)
            .HasColumnName("concurrency_token")
            .IsRowVersion();
    }
}
