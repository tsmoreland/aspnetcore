namespace MicroShop.Integrations.MessageBus.Abstractions;

public class MessageBusOptions
{
    public MessageBusType BusType { get; set; } = MessageBusType.Azure;

    public string QueueName { get; set; } = string.Empty;
}

