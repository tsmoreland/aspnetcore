namespace GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

public sealed record class EventListViewModel(Guid EventId, string Name, DateTime Date, string? ImageUrl);
