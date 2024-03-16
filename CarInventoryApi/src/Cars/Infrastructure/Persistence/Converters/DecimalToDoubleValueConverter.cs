using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarInventory.Cars.Infrastructure.Persistence.Converters;

public sealed class DecimalToDoubleValueConverter : ValueConverter<decimal, double>
{
    /// <inheritdoc />
    public DecimalToDoubleValueConverter()
        : base(static dec => Convert.ToDouble(dec), static doub => new decimal(doub))
    {
    }
}
