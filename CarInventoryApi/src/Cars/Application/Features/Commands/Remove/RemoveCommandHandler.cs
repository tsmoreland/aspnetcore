using CarInventory.Cars.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarInventory.Cars.Application.Features.Commands.Remove;

public sealed class RemoveCommandHandler(ICarRepository repository, ILogger<RemoveCommandHandler> logger) : IRequestHandler<RemoveCommand>
{
    private readonly ICarRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RemoveCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task Handle(RemoveCommand request, CancellationToken cancellationToken)
    {
        bool removed = await _repository.DeleteCarById(request.Id, cancellationToken);
        if (!removed)
        {
            _logger.LogInformation("Car matching {Id} not found", request.Id);
        }
    }
}
