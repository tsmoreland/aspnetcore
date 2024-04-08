namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Request;

public sealed record class ApplyCouponDto(int CartHeaderId, string CouponCode);
