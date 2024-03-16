//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

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
        Car? car = await repository.GetCarById(Guid.Empty, cancellationToken);
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
