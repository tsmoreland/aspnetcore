using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Persistence.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    /// <inheritdoc />
    public EventRepository(GlobalTicketDbContext dbContext, IQueryableToEnumerableConverter queryableToEnumerableConverter)
        : base(dbContext, queryableToEnumerableConverter)
    {
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEventNameAndDateUnique(string name, DateTime eventDate, CancellationToken cancellationToken)
    {
        bool matches = DataSet.Any(e => e.Name.Equals(name) && e.Date.Equals(eventDate));
        return ValueTask.FromResult(matches);
    }
}
