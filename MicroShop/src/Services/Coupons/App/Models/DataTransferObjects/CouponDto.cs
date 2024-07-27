namespace MicroShop.Services.Coupons.App.Models.DataTransferObjects;
public sealed record class CouponDto(int Id, string Code, double DiscountAmount, int MinimumAmount)
{
    public CouponDto(Coupon model)
        : this(model.Id, model.Code, model.DiscountAmount, model.MinimumAmount)
    {
    }

    public Coupon ToCoupon() => new(Id, Code, DiscountAmount, MinimumAmount);
}
