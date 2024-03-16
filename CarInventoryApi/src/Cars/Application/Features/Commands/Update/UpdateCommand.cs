using CarInventory.Cars.Application.Features.Shared;
using MediatR;

namespace CarInventory.Cars.Application.Features.Commands.Update;

public sealed record class UpdateCommand(int HorsePower, decimal FuelCapacityInLitres, int NumberOfDoors, decimal MpG) : IRequest<CarDetails?>;
