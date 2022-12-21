using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface IEventRepository : IAsyncRepository<Event>
{
    ValueTask<bool> IsEventNameAndDateUnique(string name, DateTime eventDate, CancellationToken cancellationToken);
}
