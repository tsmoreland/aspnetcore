using CarInventory.Domain.Validation;

namespace CarInventory.Domain.Models;

public sealed class Car
{
    private int _horsePower;
    private decimal _fuelCapacityInLitres;
    private int _numberOfDoors;
    private decimal _mpg;

    public Car(string make, string model, int horsePower, EngineType engine, decimal fuelCapacityInLitres, int numberOfDoors, decimal mpg)
        : this(Guid.NewGuid(), make, model, horsePower, engine, fuelCapacityInLitres, numberOfDoors, mpg)
    {
        CarValidator.ThrowIfMakeIsInvalid(make);
        CarValidator.ThrowIfModelIsInvalid(model); 
        CarValidator.ThrowIfHorsePowerIsInvalid(horsePower);
        _fuelCapacityInLitres = CarValidator.ThrowIfFuelCapacityInLitresIsInvalid(fuelCapacityInLitres);
        CarValidator.ThrowIfNumberOfDoorsIsInvalid(numberOfDoors);
        _mpg = CarValidator.ThrowIfMpGIsInvalid(mpg);
    }

    internal Car(Guid id, string make, string model, int horsePower, EngineType engine, decimal fuelCapacityInLitres, int numberOfDoors, decimal mpg) 
    {
        Id  = id;
        Make = make;
        Model = model;
        _horsePower = horsePower;
        Engine = engine;
        _fuelCapacityInLitres = fuelCapacityInLitres;
        _numberOfDoors = numberOfDoors;
        _mpg = mpg;
    }

    /// <summary>
    /// used only by EF Core, zero values will be replaced by property or field setters 
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    private Car(Guid id, string make, string model, int horsePower, EngineType engine, int numberOfDoors) 
        : this(id, make, model, horsePower, engine, decimal.Zero, numberOfDoors, decimal.Zero)
    {
    }

    public Guid Id { get; private set; }  

    public string Make { get; private set; }

    public string Model { get; private set; }

    public int HorsePower 
    {
        get => _horsePower;
        set => _horsePower = CarValidator.ThrowIfHorsePowerIsInvalid(value);
    }

    public EngineType Engine { get; private set; }

    public decimal FuelCapacityInLitres
    {
        get => _fuelCapacityInLitres;
        set => _fuelCapacityInLitres = CarValidator.ThrowIfFuelCapacityInLitresIsInvalid(value);
    }
    public int NumberOfDoors 
    {
        get => _numberOfDoors;
        set => _numberOfDoors = CarValidator.ThrowIfNumberOfDoorsIsInvalid(value);
    }

    public decimal MpG 
    {
        get => _mpg;
        set => _mpg = CarValidator.ThrowIfMpGIsInvalid(value);
    }

    public decimal GetImperialRange(decimal currentFuel)
    {
        decimal litresToGallon = new(4.546);
        decimal currentFuelInGallons = currentFuel / litresToGallon;
        return currentFuelInGallons * _mpg;
    }
    public decimal GetAmericanRange(decimal currentFuel)
    {
        decimal litresToGallon = new(3.785);
        decimal currentFuelInGallons = currentFuel / litresToGallon;
        return currentFuelInGallons * _mpg;
    }
}
