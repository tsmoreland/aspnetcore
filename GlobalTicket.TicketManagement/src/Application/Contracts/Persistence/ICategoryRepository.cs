using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface ICategoryRepository : IAsyncRepository<Category>
{
    ValueTask<Page<Category>> GetPage(int pageNumber, int pageSize, bool includeEvents, CancellationToken cancellationToken);
}
