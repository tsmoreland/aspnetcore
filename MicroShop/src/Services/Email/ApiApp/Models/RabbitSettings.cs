using System.Diagnostics.CodeAnalysis;

namespace MicroShop.Services.Email.ApiApp.Models;

public sealed class RabbitSettings(IEnumerable<RabbitQueue> queues)
{
    public RabbitSettings() : this([])
    {
    }

    public IEnumerable<RabbitQueue> Queues { get; set; } = queues;

    public bool TryGetQueueByName(string name, [NotNullWhen(true)] out RabbitQueue? queue)
    {
        queue = Queues.FirstOrDefault(q => string.Equals(q.Name, name, StringComparison.OrdinalIgnoreCase));
        return queue is not null;
    }
}
