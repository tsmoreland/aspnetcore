using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Cart;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface ICartService
{
    Task<ResponseDto<CartSummaryDto>?> Upsert( UpsertCartDto item, CancellationToken cancellationToken = default);
    Task<ResponseDto?> RemoveFromCart(int cartDetailsId, CancellationToken cancellationToken = default);
    Task<ResponseDto<CartSummaryDto>?> GetCartForCurrentUser(CancellationToken cancellationToken = default);
    Task<ResponseDto?> ApplyCoupon(int cartHeaderId, string? couponCode, CancellationToken cancellationToken = default);
    Task<ResponseDto?> RemoveCoupon(int cartHeaderId, CancellationToken cancellationToken = default);
}
