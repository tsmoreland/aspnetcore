namespace MicroShop.Integrations.MessageConsumer.Abstractions;

public interface IMessageHandler
{
    ValueTask HandleMessage(BinaryData messageBody);
}
