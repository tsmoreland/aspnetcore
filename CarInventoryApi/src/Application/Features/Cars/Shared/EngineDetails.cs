using CarInventory.Domain.Models;

namespace CarInventory.Application.Features.Cars.Shared;

public sealed record EngineDetails(EngineType Engine, decimal FuelCapacity, decimal MpG, int HorsePower);
