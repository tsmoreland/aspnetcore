using CarInventory.Application.Features.Cars.Commands.Update;

namespace CarInventory.Application.Test.Features.Cars.Commands.Update;

public sealed class UpdateCommandTest
{
    [Fact]
    public void UpdateCommand_ReturnsJsonExcludingId_WhenSerializedToJson()
    {
        Guid id = Guid.NewGuid();
        UpdateCommand command = new(id, 1, new decimal(3.2), 2, new decimal(47.5));

        string json = JsonSerializer.Serialize(command);
        UpdateCommand? deserialized = JsonSerializer.Deserialize<UpdateCommand>(json);

        deserialized?.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void UpdateCommand_DeserialziedObjectMatchesOriginalExceptForId_WhenDeserialziedFromSerializedJson()
    {
        Guid id = Guid.NewGuid();
        UpdateCommand command = new(id, 1, new decimal(3.2), 2, new decimal(47.5));

        string json = JsonSerializer.Serialize(command);
        UpdateCommand? deserialized = JsonSerializer.Deserialize<UpdateCommand>(json);

        Assert.Multiple(
            () => deserialized.Should().NotBeNull(),
            () => deserialized!.HorsePower.Should().Be(command.HorsePower),
            () => deserialized!.FuelCapacityInLitres.Should().Be(command.FuelCapacityInLitres),
            () => deserialized!.NumberOfDoors.Should().Be(command.NumberOfDoors),
            () => deserialized!.MpG.Should().Be(command.MpG));
    }
}
