namespace MicroShop.Services.Auth.App.Services.Contracts;

public interface IRabbitAuthSender
{
    void SendMessage<T>(T message, string queueName);
}
