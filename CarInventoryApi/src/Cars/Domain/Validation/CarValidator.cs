using CarInventory.Cars.Domain.Models;
using FluentValidation;

namespace CarInventory.Cars.Domain.Validation;

public sealed class CarValidator : AbstractValidator<Car>
{
    /// <inheritdoc />
    public CarValidator()
    {
        RuleFor(x => x.Make)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Make cannot be empty or greater than 50 characters in length.");
        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Model cannot be empty or greater than 50 characters in length.");
        RuleFor(x => x.HorsePower)
            .Must(value => value > 0)
            .WithMessage("Must be greater than zero");
        RuleFor(x => x.FuelCapacityInLitres)
            .Must(value => value > decimal.Zero)
            .WithMessage("Must be greater than zero");
        RuleFor(x => x.NumberOfDoors)
            .Must(value => value > 0)
            .WithMessage("Must be greater than zero");
        RuleFor(x => x.MpG)
            .Must(value => value > decimal.Zero)
            .WithMessage("Must be greater than zero");
    }

    public static string ThrowIfMakeIsInvalid(string make) => ThrowIfStringIsInvalid(make, nameof(make)); 
    public static string ThrowIfModelIsInvalid(string model) => ThrowIfStringIsInvalid(model, nameof(model));
    public static int ThrowIfHorsePowerIsInvalid(int horsePower) => ThrowIfIntIsInvalid(horsePower, nameof(horsePower));
    public static decimal ThrowIfFuelCapacityInLitresIsInvalid(decimal fuelCapacityInLitres) => ThrowIfDecimalIsInvalid(fuelCapacityInLitres, nameof(fuelCapacityInLitres));
    public static int ThrowIfNumberOfDoorsIsInvalid(int numberOfDoors) => ThrowIfIntIsInvalid(numberOfDoors, nameof(numberOfDoors));
    public static decimal ThrowIfMpGIsInvalid(decimal mpg) => ThrowIfDecimalIsInvalid(mpg, nameof(mpg));

    private static string ThrowIfStringIsInvalid(string value, string propertyName)
    {
        return value is { Length: > 0 }
           ? value
           : throw new ValidationException($"{propertyName} cannot be empty or greater than 50 characters in length.");
    }
    private static int ThrowIfIntIsInvalid(int value, string propertyName)
    {
        return value > 0
           ? value
           : throw new ValidationException($"{propertyName} must be greater than zero.");
    }
    private static decimal ThrowIfDecimalIsInvalid(decimal value, string propertyName)
    {
        return value > decimal.Zero
           ? value
           : throw new ValidationException($"{propertyName} must be greater than zero.");
    }
}
