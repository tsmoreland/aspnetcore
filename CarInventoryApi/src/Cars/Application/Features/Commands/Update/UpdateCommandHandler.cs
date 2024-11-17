using CarInventory.Cars.Application.Features.Shared;
using CarInventory.Cars.Domain.Contracts;
using CarInventory.Cars.Domain.Models;
using MediatR;

namespace CarInventory.Cars.Application.Features.Commands.Update;

public sealed class UpdateCommandHandler(ICarRepository repository) : IRequestHandler<UpdateCommand, CarDetails?>
{
    /// <inheritdoc />
    public async Task<CarDetails?> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        // broken as it's destined to go away
        var car = await repository.GetCarById(Guid.Empty, cancellationToken).ConfigureAwait(false);
        if (car is null)
        {
            return null;
        }

        (int horsePower, decimal fuelCapacityInLitres, int numberOfDoors, decimal mpg) = request;

        car.HorsePower = horsePower;
        car.FuelCapacityInLitres = fuelCapacityInLitres;
        car.NumberOfDoors = numberOfDoors;
        car.MpG = mpg;

        return new CarDetails(car);
    }
}
