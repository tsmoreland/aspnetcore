using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Domain.Entities;

public sealed class Category : AuditableEntity
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Event>? Events { get; set; }
}
