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

public sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.LastName);
        builder.HasIndex(e => e.Email);
        builder.HasIndex(e => e.PostCode);

        builder.Property(e => e.FirstName)
            .HasColumnName("first_name")
            .HasField("_firstName")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(e => e.LastName)
            .HasColumnName("last_name")
            .HasField("_lastName")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(e => e.AddressLine1)
            .HasColumnName("address_line_1")
            .HasField("_addressLine1")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(150)
            .IsRequired();
        builder.Property(e => e.AddressLine2)
            .HasColumnName("address_line_2")
            .HasField("_addressLine2")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(150);
        builder.Property(e => e.PostCode)
            .HasColumnName("post_code")
            .HasField("_postCode")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(e => e.City)
            .HasColumnName("city")
            .HasField("_city")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(e => e.PhoneNumber)
            .HasColumnName("phone_number")
            .HasField("_phoneNumber")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(15)
            .IsRequired();
        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasField("_email")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(150)
            .IsRequired();
        builder.Property(e => e.OrderTotal)
            .HasColumnName("order_total")
            .HasPrecision(18, 2)
            .IsRequired();
        builder.Property(e => e.OrderPlaced)
            .HasColumnName("order_placed")
            .IsRequired();

        builder.HasMany(e => e.OrderDetails)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.ConcurrencyToken)
            .HasColumnName("concurrency_token")
            .IsRowVersion();
    }
}
