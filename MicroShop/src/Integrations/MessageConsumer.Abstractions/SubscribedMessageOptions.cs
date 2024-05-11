namespace MicroShop.Integrations.MessageConsumer.Abstractions;

public sealed class SubscribedMessageOptions
{
    public string Name { get; set; } = string.Empty;
    public TopicSubscriptionType Type { get; set; } = TopicSubscriptionType.Email;
}
