namespace MicroShop.Services.Coupons.CouponApiApp.Models;

public sealed class Coupon(int id, string code, double discountAmount, int minimumAmount)
{
    public int Id { get; init; } = id;
    public string Code { get; set; } = code ?? string.Empty;
    public double DiscountAmount { get; set; } = discountAmount;
    public int MinimumAmount { get; set; } = minimumAmount;
}
