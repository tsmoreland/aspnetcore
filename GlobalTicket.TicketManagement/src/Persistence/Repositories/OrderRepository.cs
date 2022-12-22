using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlobalTicket.TicketManagement.Persistence.Repositories;

public sealed class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    /// <inheritdoc />
    public OrderRepository(GlobalTicketDbContext dbContext)
        : base(dbContext)
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
