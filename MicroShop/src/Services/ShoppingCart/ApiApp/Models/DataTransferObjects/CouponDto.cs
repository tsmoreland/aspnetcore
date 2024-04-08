namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

public sealed record class CouponDto(int Id, string Code, double DiscountAmount, int MinimumAmount)
{
    public static CouponDto Empty() => new(0, string.Empty, 0.0, int.MaxValue);
}
