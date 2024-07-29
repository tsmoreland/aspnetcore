namespace MicroShop.Services.ShoppingCart.App.Models;

public sealed class RabbitQueue(string queueName, string exchangeName)
{
    public RabbitQueue() : this(string.Empty, string.Empty)
    {
    }

    public string QueueName { get; set; } = queueName;
    public string ExchangeName { get; set; } = exchangeName;
}
