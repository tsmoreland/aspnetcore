using CarInventory.Cars.Application.Features.Shared;
using CarInventory.Cars.Domain.Models;
using MediatR;

namespace CarInventory.Cars.Application.Features.Commands.Add;

public sealed record class AddCommand(string Make, string Model, int HorsePower, EngineType EngineType, decimal FuelCapacityInLitres, int NumberOfDoors, decimal MpG) : IRequest<CarDetails>;
