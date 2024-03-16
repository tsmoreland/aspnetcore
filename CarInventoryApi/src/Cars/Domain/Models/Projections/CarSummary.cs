namespace CarInventory.Cars.Domain.Models.Projections;

public sealed record class CarSummary(Guid Id, string Make, string Model, EngineType EngineType, int HorsePower, int NumberOfDoors);
