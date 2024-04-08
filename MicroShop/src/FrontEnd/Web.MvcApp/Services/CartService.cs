using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Cart;
using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Services;

public sealed class CartService(IBaseService baseService) : ICartService
{
    private const string ClientName = "ShoppingCartApi";


    /// <inheritdoc />
    public async Task<ResponseDto<CartSummaryDto>?> Upsert(UpsertCartDto item, CancellationToken cancellationToken = default) =>
        await SendAsync<CartSummaryDto>(new RequestDto(ApiType.Post, "/api/cart", item));

    /// <inheritdoc />
    public async Task<ResponseDto?> RemoveFromCart(int cartDetailsId, CancellationToken cancellationToken = default) =>
        await SendAsync(new RequestDto(ApiType.Delete, $"/api/cart/{cartDetailsId}", null));

    /// <inheritdoc />
    public async Task<ResponseDto<CartSummaryDto>?> GetCartForCurrentUser(CancellationToken cancellationToken = default) => 
        await SendAsync<CartSummaryDto>(new RequestDto($"/api/cart", null));

    /// <inheritdoc />
    public async Task<ResponseDto?> ApplyCoupon(int cartHeaderId, string? couponCode, CancellationToken cancellationToken = default) =>
        await SendAsync(new RequestDto($"/api/cart/{cartHeaderId}/coupon", couponCode));

    /// <inheritdoc />
    public async Task<ResponseDto?> RemoveCoupon(int cartHeaderId, CancellationToken cancellationToken = default) =>
        await SendAsync(new RequestDto(ApiType.Delete, $"/api/cart/{cartHeaderId}/coupon", null));

    private Task<ResponseDto<T>?> SendAsync<T>(RequestDto data) => baseService.SendAsync<T>(ClientName, data);
    private Task<ResponseDto?> SendAsync(RequestDto data) => baseService.SendAsync(ClientName, data);
}
