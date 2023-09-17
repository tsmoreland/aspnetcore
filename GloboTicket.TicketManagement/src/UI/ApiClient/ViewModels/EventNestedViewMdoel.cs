namespace GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

public sealed record class EventNestedViewMdoel(Guid EventId, string Name, int Price, string? Artist, DateTime Date, Guid CategoryId);
