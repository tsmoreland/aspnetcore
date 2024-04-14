using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroShop.Integrations.MessageConsumer.AzureMessageConsumer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureMessageConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureMessageBusOptions>(configuration.GetSection("AzureMessageBus"));
        services.AddHostedService<MessageConsumer>();

        return services;
    }
}
