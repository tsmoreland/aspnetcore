using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Orders;
using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Services;

public sealed class OrderService(IBaseService baseService) : IOrderService
{
    private const string ClientName = "OrderApi";

    /// <inheritdoc />
    public async Task<ResponseDto<OrderSummaryDto>?> CreateOrder(CreateOrderDto model, CancellationToken cancellationToken = default) =>
        await SendAsync<OrderSummaryDto>(new RequestDto(ApiType.Post, "/api/orders", model));

    /// <inheritdoc />
    public async Task<ResponseDto<StripeResponseDto>?> CreateStripeSession(StripeRequest model, CancellationToken cancellationToken = default) => 
        await SendAsync<StripeResponseDto>(new RequestDto(ApiType.Post, "/api/orders/stripe", model));

    /// <inheritdoc />
    public async Task<ResponseDto<OrderStatusDto>?> GetOrderStatus(int orderId, CancellationToken cancellationToken = default) => 
        await SendAsync<OrderStatusDto>(new RequestDto($"/api/orders/{orderId}"));

    /// <inheritdoc />
    public async Task<ResponseDto<IAsyncEnumerable<OrderStatusDto>>?> GetOrderStatus(CancellationToken cancellationToken = default) =>
        await SendAsync<IAsyncEnumerable<OrderStatusDto>>(new RequestDto($"/api/orders"));

    /// <inheritdoc />
    public async Task<ResponseDto?> UpdateOrderStatus(int orderId, OrderUpdateStatus status, CancellationToken cancellationToken = default) =>
        await SendAsync<OrderSummaryDto>(new RequestDto(ApiType.Post, $"/api/orders/{orderId}", status)); 

    private Task<ResponseDto<T>?> SendAsync<T>(RequestDto data) => baseService.SendAsync<T>(ClientName, data);
}
