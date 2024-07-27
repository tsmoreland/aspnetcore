using MicroShop.Services.Orders.App.Models;

namespace MicroShop.Services.Orders.App.Services.Contracts;

public interface IRabbitMessageSender
{
    void SendMessage<T>(T message, RabbitExchange exchange);
}
