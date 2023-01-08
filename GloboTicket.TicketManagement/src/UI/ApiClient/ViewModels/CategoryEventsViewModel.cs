namespace GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

public sealed record class CategoryEventsViewModel(Guid CategoryId, string Name, ICollection<EventNestedViewMdoel> Events);
