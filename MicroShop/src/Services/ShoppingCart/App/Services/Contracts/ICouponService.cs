using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;

namespace MicroShop.Services.ShoppingCart.App.Services.Contracts;

public interface ICouponService
{
    Task<CouponDto?> GetCouponByCode(string couponCode, CancellationToken cancellationToken = default);
}
