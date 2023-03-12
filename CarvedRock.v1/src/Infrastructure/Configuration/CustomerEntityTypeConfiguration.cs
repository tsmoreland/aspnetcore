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

using System.Globalization;
using CarvedRock.Domain.Entities;
using CarvedRock.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarvedRock.Infrastructure.Configuration;

public sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(e => e.Id);
        builder.HasAlternateKey(e => e.Username);

        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(e => e.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode();
        builder.Property(e => e.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode();
        builder.Property(e => e.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode();
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);
#if DO_NOT_USE_CONVENTION_CONVERTER
            .HasConversion(
                v => v.ToString(),
                v => !string.IsNullOrWhiteSpace(v)
                    ? Enum.Parse<Status>(v)
                    : Status.Pending);
        // or simplified to .HasConversion<string>();
#endif


        builder.Property<byte[]>("Checksum")
            .HasColumnName("checksum")
            .HasComputedColumnSql("CONVERT(VARBINARY(1024),CHECKSUM([username],[first_name],[last_name]))");

        builder.IndexerProperty<DateTime>("lastUpdatedBacking")
            .HasColumnName("last_updated")
            .HasDefaultValue(new DateTime(2000, 1, 1));
        builder.IndexerProperty<string>("ConsumerTypeBacking")
            .HasColumnName("consumer_type");

        builder.Ignore(e => e.ConsumerType);
        builder.Ignore(e => e.LastUpdated);
        builder.Ignore(e => e.FullName);
        builder.Ignore(e => e.OrderedFullName);

        ConfigureOwnedAddress(builder.OwnsOne(e => e.ShipToAddress), "shipto");
        ConfigureOwnedAddress(builder.OwnsOne(e => e.BillToAddress), "billto");

        builder
            .HasMany(e => e.Orders)
            .WithOne(e => e.Customer)
            .HasForeignKey(e => e.CustomerId);
    }

    private static void ConfigureOwnedAddress(OwnedNavigationBuilder<Customer, Address> builder, string columnPrefix)
    {
        builder.Property(o => o.Street)
            .HasColumnName($"{columnPrefix}_street")
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode();
        builder.Property(o => o.City)
            .HasColumnName($"{columnPrefix}_city")
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode();
        builder.Property(o => o.Country)
            .HasColumnName($"{columnPrefix}_country")
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode();
    }
}
