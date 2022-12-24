using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Application.Contracts.Persistence;

public interface IEventRepository : IAsyncRepository<Event>
{
    ValueTask<bool> IsEventNameAndDateUnique(string name, DateTime eventDate, CancellationToken cancellationToken);
}
