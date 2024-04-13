namespace MicroShop.Integrations.MessageBus.Abstractions;

public interface IMessageBus
{
    ValueTask PublishMessage<T>(string name, T content);
}
