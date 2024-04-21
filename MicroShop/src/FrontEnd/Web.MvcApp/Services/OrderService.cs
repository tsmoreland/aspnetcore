using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Cart;
using MicroShop.Web.MvcApp.Models.Orders;
using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Services;

public sealed class OrderService(IBaseService baseService) : IOrderService
{
    private const string ClientName = "OrderApi";

    /// <inheritdoc />
    public async Task<ResponseDto<OrderSummaryDto>?> Create(CartSummaryDto cart, CancellationToken cancellationToken = default) =>
        await SendAsync<OrderSummaryDto>(new RequestDto(ApiType.Post, "/api/orders", cart));

    private Task<ResponseDto<T>?> SendAsync<T>(RequestDto data) => baseService.SendAsync<T>(ClientName, data);
    private Task<ResponseDto?> SendAsync(RequestDto data) => baseService.SendAsync(ClientName, data);
}
