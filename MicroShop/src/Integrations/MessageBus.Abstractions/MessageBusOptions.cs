namespace MicroShop.Integrations.MessageBus.Abstractions;

public class MessageBusOptions
{
    public static readonly string SectionName = "MessageBus";

    public MessageBusType BusType { get; set; } = MessageBusType.Azure;
}
