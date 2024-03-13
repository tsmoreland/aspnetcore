using System.Text.Json.Serialization;
using CarInventory.Cars.Application.Features.Shared;

namespace CarInventory.App.Configuration;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(CarDetails))]
[JsonSerializable(typeof(EngineDetails))]
public sealed partial class SerializerContext : JsonSerializerContext
{
}
