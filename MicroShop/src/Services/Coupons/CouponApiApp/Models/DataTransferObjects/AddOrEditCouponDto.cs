namespace MicroShop.Services.Coupons.CouponApiApp.Models.DataTransferObjects;

public sealed record class AddOrEditCouponDto(string Code, double DiscountAmount, int MinimumAmount)
{
    public Coupon ToNewCoupon() => new(0, Code, DiscountAmount, MinimumAmount);
    public Coupon ToExistingCoupon(int id) => new(id, Code, DiscountAmount, MinimumAmount);
}
