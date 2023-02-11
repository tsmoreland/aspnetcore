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

using GloboTicket.Shop.Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.Configuration;

public sealed class ConcertEntityTypeConfiguration : IEntityTypeConfiguration<Concert>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Concert> builder)
    {
        builder.HasKey(e => e.ConcertId);

        builder.Property(e => e.Name).HasMaxLength(Concert.MaxNameLength).IsRequired();
        builder.Property(e => e.Price).IsRequired();
        builder.Property(e => e.Artist).HasMaxLength(Concert.MaxArtistLength).IsRequired();
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.ImageUrl).IsRequired(false);
        builder.Property(e => e.ConcurrencyToken).IsRequired().IsConcurrencyToken();

        AuditDetailsConfigurer.Configure(builder.OwnsOne(e => e.Audit));

    }
}
