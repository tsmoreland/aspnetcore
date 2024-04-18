namespace MicroShop.Integrations.MessageConsumer.Abstractions;

public interface IMessageHandler
{
    ValueTask HandleMessage(MessageType messageType, BinaryData messageBody);
}
