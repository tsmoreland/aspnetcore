using MicroShop.Integrations.MessageBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MicroShop.Integrations.MessageBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<MessageBusOptions>(configuration.GetSection(MessageBusOptions.SectionName))
            .AddScoped<AzureMessageBus.MessageBus>()
            .AddScoped(BuildFromServices); 
        return services;

    }
    private static IMessageBus BuildFromServices(IServiceProvider serviceProvider)
    {
        IOptions<MessageBusOptions> optionsContainer = serviceProvider.GetRequiredService<IOptions<MessageBusOptions>>();
        MessageBusOptions options = optionsContainer.Value;

        return options.Type switch
        {
            MessageBusType.Azure => serviceProvider.GetRequiredService<AzureMessageBus.MessageBus>(),
            _ => throw new KeyNotFoundException($"Unknown message bus type {options.Type}")
        }; ; ;
    }
}
