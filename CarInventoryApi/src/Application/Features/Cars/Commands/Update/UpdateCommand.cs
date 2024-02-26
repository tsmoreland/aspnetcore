using System.Text.Json.Serialization;
using CarInventory.Application.Features.Cars.Shared;
using MediatR;

namespace CarInventory.Application.Features.Cars.Commands.Update;

public sealed record class UpdateCommand([property: JsonIgnore] Guid Id, int HorsePower, decimal FuelCapacityInLitres, int NumberOfDoors, decimal MpG) : IRequest<CarDetails?>;
