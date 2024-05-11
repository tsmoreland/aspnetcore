namespace MicroShop.Integrations.MessageConsumer.Abstractions;

public interface ISubscriptionHandler
{
    ValueTask HandleMessage(TopicSubscriptionType subscriptionType, BinaryData messageBody);
}
