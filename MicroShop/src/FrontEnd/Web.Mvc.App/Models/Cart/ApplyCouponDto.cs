namespace MicroShop.Web.Mvc.App.Models.Cart;

public sealed record class ApplyCouponDto(int CartHeaderId, string CouponCode);
