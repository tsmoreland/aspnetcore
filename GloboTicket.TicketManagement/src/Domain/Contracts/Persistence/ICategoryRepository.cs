using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Domain.Contracts.Persistence;

public interface ICategoryRepository : IAsyncRepository<Category>
{
    ValueTask<Page<Category>> GetPage(int pageNumber, int pageSize, bool includeEvents, CancellationToken cancellationToken);
}
