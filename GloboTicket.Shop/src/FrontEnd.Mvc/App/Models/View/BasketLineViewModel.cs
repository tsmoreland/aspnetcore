namespace GloboTicket.FrontEnd.Mvc.App.Models.View;

public sealed class BasketLineViewModel
{
    public Guid LineId { get; init; }
    public Guid ConcertId { get; init; }
    public string ConcertName { get; init; } = string.Empty;
    public DateTimeOffset Date { get; init; }
    public int Price { get; init; }
    public int Quantity { get; init; }
}
