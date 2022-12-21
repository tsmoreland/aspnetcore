using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;

public sealed record class UpdateEventCommand(
    Guid EventId,
    string Name,
    int Price,
    string? Artist,
    DateTime Date,
    string? Description,
    string? ImageUrl,
    Guid CategoryId) : IRequest;

