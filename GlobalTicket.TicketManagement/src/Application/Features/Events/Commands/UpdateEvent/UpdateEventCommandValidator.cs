using FluentValidation;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;

public sealed class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    private readonly IEventRepository _eventRepository;

    /// <inheritdoc />
    public UpdateEventCommandValidator(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        RuleFor(p => p.Date)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .GreaterThan(DateTime.Now);
        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .GreaterThan(0);

        RuleFor(e => e)
            .MustAsync(EventNameAndDateUnique)
            .WithMessage("An event with the same name and date already exists.");
    }

    private async Task<bool> EventNameAndDateUnique(UpdateEventCommand @event, CancellationToken cancellationToken)
    {
        return !await _eventRepository.IsEventNameAndDateUnique(@event.Name, @event.Date, cancellationToken);
    }
}
