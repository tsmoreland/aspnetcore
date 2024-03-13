using CarInventory.Cars.Domain.Models;

namespace CarInventory.Cars.Application.Features.Shared;

public sealed record EngineDetails(EngineType Engine, decimal FuelCapacity, decimal MpG, int HorsePower);
