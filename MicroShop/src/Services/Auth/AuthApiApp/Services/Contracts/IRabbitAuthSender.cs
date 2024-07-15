namespace MicroShop.Services.Auth.AuthApiApp.Services.Contracts;

public interface IRabbitAuthSender
{
    void SendMessage<T>(T message, string queueName);
}
