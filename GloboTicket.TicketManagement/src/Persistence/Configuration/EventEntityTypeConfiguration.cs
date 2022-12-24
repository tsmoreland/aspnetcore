using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboTicket.TicketManagement.Persistence.Configuration;

public sealed class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.EventId);
        builder.Property(e => e.EventId)
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.Date)
            .IsRequired();
        builder.Property(e => e.Price)
            .IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(e => e.Events)
            .HasForeignKey(e => e.CategoryId);
    }
}
