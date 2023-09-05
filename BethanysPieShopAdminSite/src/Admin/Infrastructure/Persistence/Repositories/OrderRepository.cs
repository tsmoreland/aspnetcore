//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Repositories;

[ReadOnlyRepository("BethanysPieShop.Admin.Domain.Models.Order", "BethanysPieShop.Admin.Domain.Projections.OrderSummary")]
[WritableRepository("BethanysPieShop.Admin.Domain.Models.Order")]
public sealed partial class OrderRepository : IOrderRepository, IOrderReadOnlyRepository
{
    private readonly AdminDbContext _dbContext;
    private DbSet<Order> Entities => _dbContext.Orders;

    public OrderRepository(AdminDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public partial IAsyncEnumerable<Order> GetAll();

    /// <inheritdoc />
    public partial IAsyncEnumerable<OrderSummary> GetSummaries();

    /// <inheritdoc />
    public partial Task<Order?> FindById(Guid id, CancellationToken cancellationToken);

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
        await _dbContext.Entry(entity).Collection(e => e.OrderDetails).LoadAsync(cancellationToken);
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

    private ValueTask ValidateAddOrThrow(Order entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
    private ValueTask ValidateUpdateOrThrow(Order entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    private ValueTask<(bool allowed, string reason)> AllowsDelete(Guid id, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult((true, string.Empty));
    }
}
