using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Cart;

namespace MicroShop.Web.Mvc.App.Services.Contracts;

public interface ICartService
{
    Task<ResponseDto<CartSummaryDto>?> Upsert(UpsertCartDto item, CancellationToken cancellationToken = default);
    Task<ResponseDto?> RemoveFromCart(int cartDetailsId, CancellationToken cancellationToken = default);
    Task<ResponseDto<CartSummaryDto>?> GetCartForCurrentUser(CancellationToken cancellationToken = default);
    Task<ResponseDto?> EmailCartToCurrentUser(CancellationToken cancellationToken = default);
    Task<ResponseDto?> ApplyCoupon(int cartHeaderId, string? couponCode, CancellationToken cancellationToken = default);
    Task<ResponseDto?> RemoveCoupon(int cartHeaderId, CancellationToken cancellationToken = default);
}
