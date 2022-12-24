using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboTicket.TicketManagement.Persistence.Configuration;

public sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => new { e.EventId, e.UserId });
        builder.Property(e => e.EventId)
            .IsRequired();
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.OrderTotal)
            .IsRequired();
        builder.Property(e => e.OrderPlaced)
            .IsRequired();
        builder.Property(e => e.OrderPaid)
            .IsRequired();
    }
}
