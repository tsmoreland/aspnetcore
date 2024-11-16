using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Repositories;

[ReadOnlyRepository("BethanysPieShop.Admin.Domain.Models.Order", "BethanysPieShop.Admin.Domain.Projections.OrderSummary", "BethanysPieShop.Admin.Domain.ValueObjects.OrdersOrder")]
[WritableRepository("BethanysPieShop.Admin.Domain.Models.Order")]
public sealed partial class OrderRepository(AdminDbContext dbContext) : IOrderRepository, IOrderReadOnlyRepository
{
#pragma warning disable IDE0051 // Remove unused private members
    private DbSet<Order> Entities => dbContext.Orders;
#pragma warning restore IDE0051 // Remove unused private members

    /// <inheritdoc />
    public partial IAsyncEnumerable<Order> GetAll(OrdersOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial IAsyncEnumerable<OrderSummary> GetSummaries(OrdersOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial ValueTask<Page<OrderSummary>> GetSummaryPage(PageRequest<OrdersOrder> request, CancellationToken cancellationToken);

    /// <inheritdoc />
    public partial Task<Order?> FindById(Guid id, CancellationToken cancellationToken);

#pragma warning disable IDE0051 // Remove unused private members
    private static IQueryable<OrderSummary> GetSummaries(IQueryable<Order> orders)
    {
        return orders
            .Select(e => new OrderSummary(e.Id, e.OrderTotal, e.OrderPlaced));
    }

    private static IQueryable<Order> GetIncludesForFind(IQueryable<Order> queryable)
    {
        return queryable.Include(e => e.OrderDetails);
    }
    private async ValueTask GetIncludesForFindTracked(Order entity, CancellationToken cancellationToken)
    {
#pragma warning restore IDE0051 // Remove unused private members
        await dbContext.Entry(entity).Collection(e => e.OrderDetails).LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public partial ValueTask Add(Order entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial ValueTask Update(Order entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial void Delete(Order entity);

    /// <inheritdoc/>
    public partial ValueTask Delete(Guid id, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public partial ValueTask SaveChanges(CancellationToken cancellationToken);

#pragma warning disable CA1822
#pragma warning disable IDE0051 // Remove unused private members
    private ValueTask ValidateAddOrThrow(Order entity, CancellationToken cancellationToken)
    {
        _ = entity;
        _ = cancellationToken;
        return ValueTask.CompletedTask;
    }
    private ValueTask ValidateUpdateOrThrow(Order entity, CancellationToken cancellationToken)
    {
        _ = entity;
        _ = cancellationToken;
        return ValueTask.CompletedTask;
    }

    private ValueTask<(bool allowed, string reason)> AllowsDelete(Guid id, CancellationToken cancellationToken)
    {
#pragma warning restore CA1822
#pragma warning restore IDE0051 // Remove unused private members
        _ = id;
        _ = cancellationToken;
        return ValueTask.FromResult((true, string.Empty));
    }
}
