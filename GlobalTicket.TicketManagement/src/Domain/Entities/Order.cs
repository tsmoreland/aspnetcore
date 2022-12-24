using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Domain.Entities;

public sealed class Order : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public int OrderTotal { get; set; }
    public DateTime OrderPlaced { get; set; }
    public bool OrderPaid { get; set; }
}
