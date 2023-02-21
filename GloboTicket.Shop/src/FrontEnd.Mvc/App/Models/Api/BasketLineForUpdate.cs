using System.ComponentModel.DataAnnotations;

namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public sealed class BasketLineForUpdate
{
    [Required]
    public required Guid LineId { get; init; }
    [Required]
    public required int TicketAmount { get; init; }

}
