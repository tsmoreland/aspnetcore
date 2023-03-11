using System.Linq.Expressions;
using CarvedRock.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarvedRock.Infrastructure.Converters;

internal sealed class StatusConverter : ValueConverter<Status, string>
{
    /// <inheritdoc />
    public StatusConverter()
        : base(
            v => v.ToString(),
            v => !string.IsNullOrWhiteSpace(v)
                ? Enum.Parse<Status>(v)
                : Status.Pending)
    {
    }
}
