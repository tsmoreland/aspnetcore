
namespace MicroShop.Integrations.MessageConsumer.Abstractions;

public sealed class MessageConsumerOptions
{
    public MessageBusType BusType { get; set; } = MessageBusType.Azure;
}
