namespace MicroShop.Web.App.Models.Coupons;

public sealed record class CouponDto(int Id, string Code, double DiscountAmount, int MinimumAmount);
