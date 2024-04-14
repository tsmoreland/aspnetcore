using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroShop.Integrations.MessageBus.AzureMessageBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<AzureMessageBusOptions>(configuration.GetSection("AzureMessageBus"))
            .AddScoped<MessageBus>();

        return services;
    }
}
