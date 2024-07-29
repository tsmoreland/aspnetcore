using RabbitMQ.Client;

namespace MicroShop.Services.Email.App.Models;

public sealed class RabbitExchange(string exchangeName, string type, bool durable, string? queueName)
{
    public RabbitExchange() : this(string.Empty, ExchangeType.Topic, false, null)
    {
    }

    public string ExchangeName { get; set; } = exchangeName;
    public string Type
    {
        get
        {
            return !ExchangeType.All().Contains(type)
                ? ExchangeType.Topic
                : type;
        }
        set => type = value;
    } 
    public bool Durable { get; set; } = durable;
    public string? QueueName { get; set; } = queueName;
}
