using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;

public interface ICouponService
{
    Task<CouponDto?> GetCouponByCode(string couponCode, CancellationToken cancellationToken = default);
}
