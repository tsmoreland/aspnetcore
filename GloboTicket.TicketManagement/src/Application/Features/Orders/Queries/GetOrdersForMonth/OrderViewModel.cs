namespace GloboTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;

public sealed record class OrderViewModel(Guid Id, int OrderTotal, DateTime OrderPlaced);
