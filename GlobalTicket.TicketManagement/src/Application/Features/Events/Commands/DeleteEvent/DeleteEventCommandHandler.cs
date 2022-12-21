using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;

public sealed class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public DeleteEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event is null)
        {
            return Unit.Value;
        }

        await _eventRepository.DeleteAsync(@event, cancellationToken);

        return Unit.Value;
    }
}
