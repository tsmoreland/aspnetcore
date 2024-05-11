namespace MicroShop.Integrations.MessageConsumer.Abstractions;

public sealed class TopicConsumerOptions
{
    public MessageBusType BusType { get; set; } = MessageBusType.Azure;
    public IReadOnlyList<SubscribedMessageOptions> Subscriptions { get; set; } = [];
}
