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

public sealed class PieEntityTypeConfiguration : IEntityTypeConfiguration<Pie>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Pie> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Price);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasField("_name")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(e => e.ShortDescription)
            .HasColumnName("short_description")
            .HasField("_shortDescription")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(100);
        builder.Property(e => e.LongDescription)
            .HasColumnName("long_description")
            .HasField("_longDescription")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(1000);
        builder.Property(e => e.AllergyInformation)
            .HasColumnName("allergy_information")
            .HasField("_allergyInformation")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(1000);
        builder.Property(e => e.Price)
            .HasColumnName("price")
            .HasField("_price")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasPrecision(18, 2);
        builder.Property(e => e.ImageFilename)
            .HasColumnName("image_filename")
            .HasField("_imageFilename")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(100);
        builder.Property(e => e.ImageThumbnailFilename)
            .HasColumnName("image_thumbnail_filename")
            .HasField("_imageThumbnailFilename")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField)
            .HasMaxLength(100);

        builder.Property(e => e.CategoryId).HasColumnName("category_id");
        builder.HasOne(e => e.Category)
            .WithMany(e => e.Pies)
            .IsRequired(false)
            .HasForeignKey(e => e.CategoryId);

        builder
            .HasMany(e => e.Ingredients);
    }
}
