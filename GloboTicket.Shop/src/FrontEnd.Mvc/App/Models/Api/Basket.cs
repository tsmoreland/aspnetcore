namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public sealed class Basket
{
    public required Guid BasketId { get; init; }
    public required Guid UserId { get; init; }
    public required int NumberOfItems { get; init; }
}
