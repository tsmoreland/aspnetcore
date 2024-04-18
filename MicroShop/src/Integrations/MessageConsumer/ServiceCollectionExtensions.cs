using MicroShop.Integrations.MessageConsumer.Abstractions;
using MicroShop.Integrations.MessageConsumer.AzureMessageConsumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroShop.Integrations.MessageConsumer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageConsumer<TMessageHandler>(this IServiceCollection services, IConfiguration configuration)
        where TMessageHandler : class, IMessageHandler
    {
        MessageConsumerOptions? options = configuration.GetRequiredSection("MessageConsumer").Get<MessageConsumerOptions>();
        services.AddSingleton<IMessageHandler, TMessageHandler>();
        return options?.BusType switch
        {
            MessageBusType.Azure => services.AddAzureMessageConsumer(configuration),
            _ => services
        };
    }
}
