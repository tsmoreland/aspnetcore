namespace MicroShop.Integrations.MessageBus.AzureMessageBus;

public sealed class AzureMessageBusOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
}
