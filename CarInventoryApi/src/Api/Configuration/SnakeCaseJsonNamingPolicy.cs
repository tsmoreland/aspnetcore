using System.Text.Json;
using CarInventory.Api.Extensions;

namespace CarInventory.Api.Configuration;

public sealed class SnakeCaseJsonNamingPolicy : JsonNamingPolicy
{
    private static readonly Lazy<SnakeCaseJsonNamingPolicy> s_instance = new(() => new SnakeCaseJsonNamingPolicy());
    public static SnakeCaseJsonNamingPolicy Instance => s_instance.Value;

    /// <inheritdoc />
    public override string ConvertName(string name) => name.ToSnakeCase();
}
