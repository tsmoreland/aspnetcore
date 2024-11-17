using CarInventory.Cars.Application.Features.Shared;
using CarInventory.Cars.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarInventory.Cars.Application.Features.Commands.Add;

public sealed class AddCommandHandler(ICarRepository repository) : IRequestHandler<AddCommand, CarDetails>
{
    /// <inheritdoc />
    public async Task<CarDetails> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var car = repository.Add(request.Make, request.Model, request.HorsePower, request.EngineType, request.FuelCapacityInLitres, request.NumberOfDoors, request.MpG);
        await repository.SaveChanges(cancellationToken).ConfigureAwait(false);
        return new CarDetails(car);
    }
}
