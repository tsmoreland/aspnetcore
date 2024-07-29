using RabbitMQ.Client;

namespace MicroShop.Services.Orders.App.Models;

public sealed class RabbitExchange(string exchangeName, string type, bool durable)
{
    public RabbitExchange() : this(string.Empty, ExchangeType.Topic, false)
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
}
