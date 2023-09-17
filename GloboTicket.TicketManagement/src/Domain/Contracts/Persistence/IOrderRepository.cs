using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Domain.Contracts.Persistence;

public interface IOrderRepository : IAsyncRepository<Order>
{

    ValueTask<Page<Order>> GetPagedOrdersForMonth(DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken);

    ValueTask<int> GetTotalCountOfOrdersForMonth(DateTime date, CancellationToken cancellationToken);
}
