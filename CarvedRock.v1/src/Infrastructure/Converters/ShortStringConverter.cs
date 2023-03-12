using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarvedRock.Infrastructure.Converters;

internal sealed class ShortStringConverter : ValueConverter<string, string>
{
    /// <inheritdoc />
    public ShortStringConverter()
        : base(
            v => v,
            v => v,
            new ConverterMappingHints(size: 100))
    {
    }
}
