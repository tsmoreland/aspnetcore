using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboTicket.TicketManagement.Persistence.Configuration;

public sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(e => e.CategoryId)
            .IsRequired();
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasMany(e => e.Events)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId);
    }
}
