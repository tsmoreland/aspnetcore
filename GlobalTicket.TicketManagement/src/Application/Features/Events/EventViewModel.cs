namespace GlobalTicket.TicketManagement.Application.Features.Events;

public sealed class EventViewModel
{
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? ImageUrl { get; set; }
}
