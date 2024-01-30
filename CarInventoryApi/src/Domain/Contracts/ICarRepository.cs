using CarInventory.Domain.Models;

namespace CarInventory.Domain.Contracts;

public interface ICarRepository
{
    Car Add(string make, string model, int horsePower, EngineType engineType, decimal fuelCapacityInLitres, int numberOfDoors, decimal mpg);
    Task<Car?> GetCarById(Guid id, CancellationToken cancellationToken);
    Task<Car?> GetCarByMakeAndModel(string make, string model, CancellationToken cancellationToken);

    IAsyncEnumerable<Car> GetAllCars();

    Task<bool> DeleteCarById(Guid id, CancellationToken cancellationToken);

    Task SaveChanges(CancellationToken cancellationToken);
}
