using MediatR;
using MicroShop.Services.Orders.ApiApp.Infrastructure.Data;
using MicroShop.Services.Orders.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrderDetailsById;

public sealed class GetOrderDetailsByIdRequestHandler(AppDbContext dbContext) : IRequestHandler<GetOrderDetailsByIdRequest, OrderHeader?>
{
    /// <inheritdoc />
    public async Task<OrderHeader?> Handle(GetOrderDetailsByIdRequest request, CancellationToken cancellationToken)
    {
        int orderId = request.OrderId;
        string? userId = request.UserId;

        IQueryable<OrderHeader> query = dbContext.OrderHeaders.AsNoTracking().Include(o => o.Items);
        query = userId is { Length: > 0 }
            ? query.Where(o => o.Id == orderId && o.UserId == userId)
            : query.Where(o => o.Id == orderId);

        OrderHeader? order = await query
            .AsAsyncEnumerable()
            .FirstOrDefaultAsync(cancellationToken);

        return order;
    }
}
