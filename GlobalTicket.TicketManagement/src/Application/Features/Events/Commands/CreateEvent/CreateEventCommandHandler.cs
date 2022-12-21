using AutoMapper;
using FluentValidation.Results;
using GlobalTicket.TicketManagement.Application.Contracts.Exceptions;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public sealed class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandHandler(IMapper mapper, IEventRepository eventRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    }

    /// <inheritdoc />
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = _mapper.Map<Event>(request);

        ValidationResult validationResult = await new CreateEventCommandValidator(_eventRepository)
            .ValidateAsync(request, cancellationToken);
        ValidationFailureException.ThrowIfHasErrors(validationResult);

        @event = await _eventRepository.AddAsync(@event, cancellationToken);
        return @event.EventId;
    }
}
