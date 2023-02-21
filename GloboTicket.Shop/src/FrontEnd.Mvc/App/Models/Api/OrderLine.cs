namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public sealed class OrderLine
{
    public required Guid ConcertId { get; init; }
    public required int TicketCount { get; init; }
    public required int Price { get; init; }
}
