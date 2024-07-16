namespace MicroShop.Services.Email.ApiApp.Models;

public sealed class RabbitQueue(string name, string queueName, string exchangeName)
{
    public RabbitQueue() : this(string.Empty, string.Empty, string.Empty)
    {
    }

    public string Name { get; set; } = name;
    public string QueueName { get; set; } = queueName;
    public string ExchangeName { get; set; } = exchangeName;
}
