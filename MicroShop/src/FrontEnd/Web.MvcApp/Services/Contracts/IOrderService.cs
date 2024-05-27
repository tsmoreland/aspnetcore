using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Orders;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface IOrderService
{
    Task<ResponseDto<OrderSummaryDto>?> CreateOrder(CreateOrderDto model, CancellationToken cancellationToken = default);
    Task<ResponseDto<StripeResponseDto>?> CreateStripeSession(StripeRequest request, CancellationToken cancellationToken = default);
    Task<ResponseDto<OrderSummaryDto>?> GetOrderSummary(int orderId, CancellationToken cancellationToken = default);
    Task<ResponseDto<OrderDto>?> GetOrder(int orderId, CancellationToken cancellationToken = default);
    Task<ResponseDto<IAsyncEnumerable<OrderDto>>?> GetOrders(string? userId = null, CancellationToken cancellationToken = default);
    Task<ResponseDto?> UpdateOrderStatus(int orderId, OrderUpdateStatus status, CancellationToken cancellationToken = default);
}
