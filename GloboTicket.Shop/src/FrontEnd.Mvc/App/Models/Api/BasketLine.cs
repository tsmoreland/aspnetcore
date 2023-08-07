namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public sealed class BasketLine
{
    public required Guid BasketLineId { get; set; }
    public required Guid BasketId { get; set; }
    public required Guid ConcertId { get; set; }
    public required int TicketAmount { get; set; }
    public required int Price { get; set; }
    public required Concert Concert { get; set; } 
}
