using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface IOrderRepository : IAsyncRepository<Order>
{

    ValueTask<Page<Order>> GetPagedOrdersForMonth(DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken);

    ValueTask<int> GetTotalCountOfOrdersForMonth(DateTime date, CancellationToken cancellationToken);
}
