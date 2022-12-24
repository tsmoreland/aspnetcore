using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    /// <inheritdoc />
    public EventRepository(GloboTicketDbContext dbContext, IQueryableToEnumerableConverter queryableToEnumerableConverter)
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
