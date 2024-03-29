namespace MicroShop.Web.MvcApp.Models.Coupons;

public sealed class AddOrEditCouponDto(string code, double discountAmount, int minimumAmount)
{
    public AddOrEditCouponDto()
        : this(string.Empty, 0.0, 0)
    {
    }

    public string Code { get; set; } = code;
    public double DiscountAmount { get; set; } = discountAmount;
    public int MinimumAmount { get; set; } = minimumAmount;
}
