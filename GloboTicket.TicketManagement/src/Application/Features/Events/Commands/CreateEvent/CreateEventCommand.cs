using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public sealed record class CreateEventCommand(
    string Name,
    int Price,
    string? Artist,
    DateTime Date,
    string? Description,
    string? ImageUrl,
    Guid CategoryId) : IRequest<Guid>
{
    /// <inheritdoc />
    public override string ToString()
    {
        return $"Event Name: {Name}; Price {Price}; By {Artist}; On {Date.ToShortDateString()}; Description: {Description}";
    }
}
