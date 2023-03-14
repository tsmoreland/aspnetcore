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
using CarvedRock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarvedRock.Infrastructure.Configuration;

public sealed class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(500);

        builder.Property(e => e.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(22,2)")
            .IsRequired();

        builder.Property(e => e.PriceAfterVat)
            .HasColumnName("price_after_vat")
            .HasColumnType("decimal(22,2)")
            .HasComputedColumnSql("[price]*1.20");

        builder.Property(e => e.Weight)
            .HasColumnName("unit_weight")
            .HasColumnType("float(36)")
            .IsRequired();

#if USING_TABLE_PER_HIERARCHY
        builder
            .HasDiscriminator<string>("ItemType")
            .HasValue<Item>("UncatagorizedItem");
        builder.Property("ItemType")
            .HasColumnName("item_type")
            .HasDefaultValue("UncatagorizedItem");

        // this part would have to go in sql model configuration or a food item entity type configuration
        // this code would configure a shared column between dried and canned food item as both have production date
        modelBinder.Entity<DriedFoodItem>().Property(e => e.ProductionDate)
            .HasColumnName("production_date")
            .IsSparse();
        modelBinder.Entity<CannedFoodItem>().Property(e => e.ProductionDate)
            .HasColumnName("production_date")
            .IsSparse(); // optimized for usually null columns
#endif

        builder.HasMany(e => e.ItemOrders)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemsId);
    }
}
