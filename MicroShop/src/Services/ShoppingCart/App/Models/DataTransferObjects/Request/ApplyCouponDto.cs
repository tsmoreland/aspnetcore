namespace MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Request;

public sealed record class ApplyCouponDto(int CartHeaderId, string CouponCode);
