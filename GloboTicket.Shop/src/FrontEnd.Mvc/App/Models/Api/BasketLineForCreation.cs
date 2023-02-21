using System.ComponentModel.DataAnnotations;

namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public sealed class BasketLineForCreation
{
    [Required]
    public required Guid ConcertId { get; init; }

    [Required]
    public required int TicketAmount { get; init; }

    [Required]
    public required int Price { get; init; }
}
