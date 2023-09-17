using System.ComponentModel.DataAnnotations;

namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public sealed class BasketLineForCreation
{
    /// <summary>
    /// Concert Id
    /// </summary>
    [Required]
    public required Guid Id { get; init; }

    /// <summary>
    /// Ticket Amount
    /// </summary>
    [Required]
    public required int TicketAmount { get; init; }

    /// <summary>
    /// Price
    /// </summary>
    [Required]
    public required int Price { get; init; }
}
