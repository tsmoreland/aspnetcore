namespace MicroShop.Integrations.MessageBus.Abstractions;

public class MessageBusOptions
{
    public static readonly string SectionName = "MessageBus";

    public MessageBusType Type { get; set; } = MessageBusType.Azure;
    public string ConnectionString { get; set; } = string.Empty;
}
