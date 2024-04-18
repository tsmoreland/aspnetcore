namespace MicroShop.Integrations.MessageConsumer.Abstractions;

public sealed class MesageQueueOptions
{
    public string Name { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.ShoppingCart;
}
