namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;

public sealed class EventViewModel
{
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? ImageUrl { get; set; }
}
