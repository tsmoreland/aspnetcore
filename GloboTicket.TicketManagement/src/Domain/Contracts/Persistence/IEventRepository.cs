using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Domain.Contracts.Persistence;

public interface IEventRepository : IAsyncRepository<Event>
{
    ValueTask<bool> IsEventNameAndDateUnique(string name, DateTime eventDate, CancellationToken cancellationToken);
}
