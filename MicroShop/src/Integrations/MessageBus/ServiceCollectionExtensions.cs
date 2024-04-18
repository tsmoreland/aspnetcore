using MicroShop.Integrations.MessageBus.Abstractions;
using MicroShop.Integrations.MessageBus.AzureMessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MicroShop.Integrations.MessageBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        MessageBusOptions? options = configuration.GetRequiredSection("MessageBus").Get<MessageBusOptions>();   

        return options?.BusType switch
        {
            MessageBusType.Azure => services.AddAzureMessageBus(configuration),
            _ => services
        };
    }
}
