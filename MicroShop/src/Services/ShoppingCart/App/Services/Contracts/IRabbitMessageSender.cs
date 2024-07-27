using MicroShop.Services.ShoppingCart.App.Models;

namespace MicroShop.Services.ShoppingCart.App.Services.Contracts;

public interface IRabbitMessageSender
{
    void SendMessage<T>(T message, RabbitQueue queue);
}
