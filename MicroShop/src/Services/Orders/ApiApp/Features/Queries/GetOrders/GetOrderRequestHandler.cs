using MediatR;
using MicroShop.Services.Orders.ApiApp.Infrastructure.Data;
using MicroShop.Services.Orders.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrders;

public sealed class GetOrderRequestHandler(AppDbContext dbContext) : IStreamRequestHandler<GetOrdersRequest, OrderHeader>
{
    /// <inheritdoc />
    public IAsyncEnumerable<OrderHeader> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        return dbContext.OrderHeaders.AsNoTracking().AsAsyncEnumerable();
    }
}
