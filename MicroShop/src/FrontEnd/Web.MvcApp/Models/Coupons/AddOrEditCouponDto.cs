namespace MicroShop.Web.MvcApp.Models.Coupons;

public sealed record class AddOrEditCouponDto(string Code, double DiscountAmount, int MinimumAmount);
