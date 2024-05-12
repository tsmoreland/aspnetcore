using MediatR;
using MicroShop.Services.Orders.ApiApp.Infrastructure.Data;
using MicroShop.Services.Orders.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrdersByUserId;

public sealed class GetOrdersByUserIdRequestHandler(AppDbContext dbContext) : IStreamRequestHandler<GetOrdersByUserIdRequest, OrderHeader>
{
    /// <inheritdoc />
    public IAsyncEnumerable<OrderHeader> Handle(GetOrdersByUserIdRequest request, CancellationToken cancellationToken)
    {
        string userId = request.UserId;
        return dbContext.OrderHeaders.AsNoTracking().Where(e => e.UserId == userId).AsAsyncEnumerable();
    }
}
