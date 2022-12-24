using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;

public sealed record class DeleteEventCommand(Guid EventId) : IRequest;
