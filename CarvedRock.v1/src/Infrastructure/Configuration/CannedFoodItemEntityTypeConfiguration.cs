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
using CarvedRock.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarvedRock.Infrastructure.Configuration;

public sealed class CannedFoodItemEntityTypeConfiguration : IEntityTypeConfiguration<CannedFoodItem>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CannedFoodItem> builder)
    {
        // no ToTable because we're using table per hierarchy

        builder.Property(e => e.CanningMaterial)
            .HasColumnName("canning_material")
            .HasConversion<CanningMaterialConverter>();

        builder.Property(e => e.ExpiryDate)
            .HasColumnName("expiry_date")
            .IsRequired();

        // because we have a name for this it will be shared between canned and dried food
        builder.Property(e => e.ProductionDate)
            .HasColumnName("production_date")
            .IsSparse();
    }
}
