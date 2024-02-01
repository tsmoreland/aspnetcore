using CarInventory.Domain.Contracts;
using CarInventory.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.Infrastructure.Persistence;

public sealed class CarRepository(CarsDbContext dbContext) : ICarRepository
{
    private readonly CarsDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc />
    public Car Add(string make, string model, int horsePower, EngineType engineType, decimal fuelCapacityInLitres, int numberOfDoors, decimal mpg)
    {
        Car car = new(make, model, horsePower, engineType, fuelCapacityInLitres, numberOfDoors, mpg);
        _dbContext.Cars.Add(car);
        return car;
    }

    /// <inheritdoc />
    public Task<Car?> GetCarById(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Cars.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Car?> GetCarByMakeAndModel(string make, string model, CancellationToken cancellationToken)
    {
        return _dbContext.Cars.AsNoTracking().FirstOrDefaultAsync(e => e.Make == make && e.Model == model, cancellationToken);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<Car> GetAllCars()
    {
        return _dbContext.Cars.AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public async Task<bool> DeleteCarById(Guid id, CancellationToken cancellationToken)
    {
        Car? car = await _dbContext.Cars.FindAsync([id], cancellationToken);
        if (car is not null)
        {
            _dbContext.Cars.Remove(car);
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

}
