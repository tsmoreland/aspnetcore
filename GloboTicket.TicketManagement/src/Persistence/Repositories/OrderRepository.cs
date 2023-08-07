using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public sealed class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    /// <inheritdoc />
    public OrderRepository(GloboTicketDbContext dbContext, IQueryableToEnumerableConverter queryableToEnumerableConverter)
        : base(dbContext, queryableToEnumerableConverter)
    {
    }

    /// <inheritdoc />
    public async ValueTask<Page<Order>> GetPagedOrdersForMonth(DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        IQueryable<Order> source = DataSet
            .AsNoTracking()
            .Where(o => o.OrderPlaced.Month == date.Month && o.OrderPlaced.Year == date.Year);

        return await GetPage(source, pageNumber, pageSize, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<int> GetTotalCountOfOrdersForMonth(DateTime date, CancellationToken cancellationToken)
    {
        return await DataSet.CountAsync(o => o.OrderPlaced.Month == date.Month && o.OrderPlaced.Year == date.Year, cancellationToken);
    }
}
