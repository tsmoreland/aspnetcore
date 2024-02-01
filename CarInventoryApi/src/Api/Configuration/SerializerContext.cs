using System.Text.Json.Serialization;
using CarInventory.Application.Features.Cars.Shared;

namespace CarInventory.Api.Configuration;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(CarDetails))]
[JsonSerializable(typeof(EngineDetails))]
public sealed partial class SerializerContext : JsonSerializerContext
{
}
