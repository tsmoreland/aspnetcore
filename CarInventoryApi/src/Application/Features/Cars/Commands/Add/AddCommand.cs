using CarInventory.Application.Features.Cars.Shared;
using CarInventory.Domain.Models;
using MediatR;

namespace CarInventory.Application.Features.Cars.Commands.Add;

public sealed record class AddCommand(string Make, string Model, int HorsePower, EngineType EngineType, decimal FuelCapacityInLitres, int NumberOfDoors, decimal MpG) : IRequest<CarDetails>;
