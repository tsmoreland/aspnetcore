﻿using CarInventory.Cars.Domain.Models;

namespace CarInventory.Cars.Application.Features.Shared;

public sealed record class CarDetails(Guid Id, string Make, string Model, EngineDetails Engine, int NumberOfDoors)
{
    public CarDetails(Car car) : this(car.Id, car.Make, car.Model, new EngineDetails(car.Engine, car.FuelCapacityInLitres, car.MpG, car.HorsePower), car.NumberOfDoors)
    {
    }
}
