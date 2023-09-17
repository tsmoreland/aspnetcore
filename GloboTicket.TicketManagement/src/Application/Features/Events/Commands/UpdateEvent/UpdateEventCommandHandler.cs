using AutoMapper;
using FluentValidation.Results;
using GloboTicket.TicketManagement.Domain.Contracts.Exceptions;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;

public sealed class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IMapper _mapper;
    private readonly IEventRepository _eventRepository;

    public UpdateEventCommandHandler(IMapper mapper, IEventRepository eventRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    }

    /// <inheritdoc />
    public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        NotFoundException.ThrowIfNull(@event, request.EventId);

        ValidationResult validationResult = await new UpdateEventCommandValidator(_eventRepository)
            .ValidateAsync(request, cancellationToken);
        ValidationFailureException.ThrowIfHasErrors(validationResult);

        _mapper.Map(request, @event, typeof(UpdateEventCommand), typeof(Event));
        await _eventRepository.UpdateAsync(@event!, cancellationToken);
    }
}
