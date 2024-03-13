using CarInventory.Cars.Domain.Models;
using CarInventory.Cars.Domain.Models.Projections;

namespace CarInventory.Cars.Domain.Contracts;

public interface ICarRepository
{
    Car Add(string make, string model, int horsePower, EngineType engineType, decimal fuelCapacityInLitres, int numberOfDoors, decimal mpg);
    Task<Car?> GetCarById(Guid id, CancellationToken cancellationToken);
    Task<Car?> GetCarByMakeAndModel(string make, string model, CancellationToken cancellationToken);

    IAsyncEnumerable<Car> GetAllCars();
    IAsyncEnumerable<CarSummary> GetCarSummaries(); // TODO: add page request / sort request, probably to shared

    Task<bool> DeleteCarById(Guid id, CancellationToken cancellationToken);

    Task SaveChanges(CancellationToken cancellationToken);
}
