namespace MicroShop.Web.MvcApp.Models.Cart;

public sealed record class ApplyCouponDto(int CartHeaderId, string CouponCode);
