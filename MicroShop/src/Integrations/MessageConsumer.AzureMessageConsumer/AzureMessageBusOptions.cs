namespace MicroShop.Integrations.MessageConsumer.AzureMessageConsumer;

public sealed class AzureMessageBusOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
}
