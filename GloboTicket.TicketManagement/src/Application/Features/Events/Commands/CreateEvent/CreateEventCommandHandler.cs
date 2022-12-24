using AutoMapper;
using FluentValidation.Results;
using GloboTicket.TicketManagement.Application.Contracts.Exceptions;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public sealed class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IEventRepository _eventRepository;
    private readonly IEmailService _emailService;

    public CreateEventCommandHandler(IMapper mapper, IEventRepository eventRepository, IEmailService emailService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    /// <inheritdoc />
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = _mapper.Map<Event>(request);

        ValidationResult validationResult = await new CreateEventCommandValidator(_eventRepository)
            .ValidateAsync(request, cancellationToken);
        ValidationFailureException.ThrowIfHasErrors(validationResult);

        @event = await _eventRepository.AddAsync(@event, cancellationToken);

        // TODO:
        // this should use another repository or better yet defer to another service to determine who
        // should be notified, would be ideal for a background service so we don't actually have to send here,
        // this could just queue the request and move on
        Email mail = new("root@127.0.0.1", $"A new event was created: {request}", "A new event was created");

        try
        {
            await _emailService.SendEmail(mail, cancellationToken);
        }
        catch
        {
            // This isn't fatal, but should at some point be logged.
        }


        return @event.EventId;
    }
}
